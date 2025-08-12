using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CryptidUtils {
    public static GameManager Instance;

    public bool isPaused = false;
    public bool isPlaying = false;
    [Tooltip("Time (in seconds) before extraction")]
    public float maxTime;
    [Tooltip("Time (in seconds) before extraction finishes")]
    public float extractionTime;
    public float TimeLeft { get; private set; }
    public bool canExtract = false;

    private int _mapsIndex = 0;
    public GameObject player;
    public GameObject cursor;

    // Registration
    [SerializeField] private List<PlayerSpawner> playerSpawners = new();
    [SerializeField] private List<EnemySpawner> enemySpawners = new();
    public List<Scene> maps = new();
    public List<Room> rooms = new();
    [Tooltip("Maximum possible score given items spawned - generated automatically")]
    public int maxScore;

    // Accumulative Data
    [Space]
    [Header("Accumulative Data")]
    public int comScore;
    [Tooltip("Time played")]
    public float comTime;
    [Tooltip("Times rounds have been completed in this save")]
    public int comRuns;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");

        MenuManager.Instance.OpenMain();
    }

    private void FixedUpdate() {
        if (isPlaying) {
            if (TimeLeft > -extractionTime) {
                TimeLeft -= Time.fixedDeltaTime;
                if (TimeLeft < 0 && canExtract)
                    StopGame(true);
            } else
                StopGame();
        }
    }

    public void TogglePause() {
        if (!isPaused) {
            Pause();
        }
        else {
            Unpause();
        }
    }

    public void Pause() {
        isPaused = true;
        UnlockCursor();
        MenuManager.Instance.OpenPause();
        PlayerManager.Instance.Pause();
        Director.Instance.Pause();
    }

    public void Unpause() {
        isPaused = false;
        LockCursor();
        MenuManager.Instance.OpenOverlay();
        PlayerManager.Instance.Play();
        Director.Instance.Play();
    }

    public void StartGame() {
        _mapsIndex = 0;
        MenuManager.Instance.OpenOverlay();
        LoadNextMap();
        HidePseudoCursor();
        TimeLeft = maxTime;
        isPlaying = true;
        canExtract = false;
    }
    public void StopGame(bool extracted = false) {
        if (extracted) { // player successfully extracted
            MenuManager.Instance.OpenRewards();
            InventoryManager.Instance.Roll();
            comScore += PlayerManager.Instance.score;
            comTime += (TimeLeft - maxTime) + maxTime;
            comRuns++;
        } else { // player failed to extract
            MenuManager.Instance.OpenMain();
            InventoryManager.Instance.Clear();
            comScore = 0;
            comTime = 0;
            comRuns = 0;
        }

        isPlaying = false;
        UnloadPlayer();
        Director.Instance.UnloadEnemies();
        CSceneManager.Instance.UnloadExclusives();
    }

    #region Loading
    public void LoadNextMap() {
        if (_mapsIndex < maps.Count) {
            CSceneManager.Instance.LoadScene(maps[_mapsIndex]);
            _mapsIndex++;
            StartCoroutine(LoadEnemies());
            if (PlayerManager.Instance == null)
                LoadPlayer();
            else
                StartCoroutine(MovePlayer(0.2f));
        } else {
            LogErr("Couldn't load next map, reached end of maps list");
        }
    }

    public void LoadPlayer() {
        if (InputManager.Instance == null) {
            LogWarn("Input manager not found, creating new input manager");
            gameObject.AddComponent<InputManager>();
        }

        Instantiate(player);
        Log("Successfully spawned player!");
        StartCoroutine(MovePlayer(0.2f));
    }
    public IEnumerator MovePlayer(float delay = 0.5f) {
        yield return new WaitForSeconds(delay);
        Log("Attempting to set player position");
        
        if (playerSpawners.Count > 0) {
            //player.transform.SetPositionAndRotation(playerSpawners[0].transform.position, playerSpawners[0].setRotation ? playerSpawners[0].transform.rotation : player.transform.rotation);
            PlayerManager.Instance.ForceMoveTo(playerSpawners[0].transform);
            //PlayerManager.Instance.agent.SetDestination(player.transform.position);
        } else
            LogWarn("No PlayerSpawners active, skipping player move");
    }

    public IEnumerator LoadEnemies(float delay = 0.5f) {
        yield return new WaitForSeconds(delay);

        //if (enemySpawners.Count > 0)
            foreach (EnemySpawner spawner in enemySpawners)
                Director.Instance.Spawn(spawner);
    }

    public void UnloadPlayer() {
        Destroy(PlayerManager.Instance.gameObject);
    }
    #endregion

    #region Registration
    public void Register(PlayerSpawner spawner) {
        playerSpawners.Add(spawner);
        Log("Registered new Player Spawner: " + spawner.gameObject.name);
    }
    public void Deregister(PlayerSpawner spawner) {
        playerSpawners.Remove(spawner);
        Log("Deregistered existing Player Spawner: " + spawner.gameObject.name);
    }

    public void Register(EnemySpawner spawner, float checkDelay = 0.2f) {
        enemySpawners.Add(spawner);
        Log("Registered new Enemy Spawner: " + spawner.gameObject.name);
    }
    public void Deregister(EnemySpawner spawner) {
        enemySpawners.Remove(spawner);
        Log("Deregistered existing Enemy Spawner: " + spawner.gameObject.name);
    }

    // uncontrolled handling of array, could possibly lead to unwanted errors.
    public void Register(Room room) {
        rooms.Add(room);
        Log("Registered new Room: " + room.name);
    }
    public void Deregister(Room room) {
        rooms.Remove(room);
        Log("Deregistered existing room: " + room.gameObject.name);
    }

    public void Register(Valuable valuable) {
        maxScore += valuable.value;
    }
    #endregion

    #region cursors
    public static void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // TODO Custom in world cursor for the console
    public static void ShowPseudoCursor() {
        Instance.cursor.SetActive(true);
    }
    public static void HidePseudoCursor() {
        Instance.cursor.SetActive(false);
    }
    #endregion
}
