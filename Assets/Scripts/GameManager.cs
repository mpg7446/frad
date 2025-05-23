using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : CryptidUtils {
    public static GameManager Instance;

    public bool isPaused = false;

    public GameObject player;
    [SerializeField] private List<PlayerSpawner> playerSpawners = new();
    [SerializeField] private List<EnemySpawner> enemySpawners = new();

    public List<Scene> maps = new();
    private int _mapsIndex = 0;

    public List<Room> rooms = new();

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");

        //LoadPlayer();
        LoadNextMap();
    }

    public void Pause() {
        isPaused = true;
    }

    public void Play() {
        isPaused = false;
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
    #endregion

    #region Registration
    public void RegisterPlayerSpawner(PlayerSpawner spawner) {
        playerSpawners.Add(spawner);
        Log("Registered new Player Spawner: " + spawner.gameObject.name);
    }
    public void DeregisterPlayerSpawner(PlayerSpawner spawner) {
        playerSpawners.Remove(spawner);
        Log("Deregistered existing Player Spawner: " + spawner.gameObject.name);
    }

    public void RegisterEnemySpawner(EnemySpawner spawner, float checkDelay = 0.2f) {
        enemySpawners.Add(spawner);
        Log("Registered new Enemy Spawner: " + spawner.gameObject.name);
    }
    public void DeregisterEnemySpawner(EnemySpawner spawner) {
        enemySpawners.Remove(spawner);
        Log("Deregistered existing Enemy Spawner: " + spawner.gameObject.name);
    }

    // uncontrolled handling of array, could possibly lead to unwanted errors.
    public void RegisterRoom(Room room) {
        rooms.Add(room);
        Log("Registered new Room: " + room.name);
    }
    public void DeregisterRoom(Room room) {
        rooms.Remove(room);
        Log("Deregistered existing room: " + room.gameObject.name);
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
    public static void UnlockPseudoCursor() {

    }
    public static void LockPseudoCursor() {

    }
    #endregion
}
