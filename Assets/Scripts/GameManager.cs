using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CryptidUtils {
    public static GameManager Instance;

    public GameObject player;
    [SerializeField] private List<PlayerSpawner> playerSpawns;

    public List<Scene> maps;
    private int mapsIndex = 0;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");

        LoadNextMap();
    }

    public void LoadPlayer(Vector3 pos) {
        try {
            InputManager ignore = InputManager.Instance;
        } catch {
            LogWarn("Input manager not found, creating new input manager");
            gameObject.AddComponent<InputManager>();
        }
        Instantiate(player).transform.position = pos;
    }
    public IEnumerable MovePlayer(float time = 1) {
        yield return new WaitForSeconds(time);

        if (playerSpawns.Count > 0) {
            player.transform.SetPositionAndRotation(playerSpawns[0].transform.position, playerSpawns[0].setRotation ? playerSpawns[0].transform.rotation : player.transform.rotation);
        } else
            LogWarn("No PlayerSpawners active, skipping player move");
    }

    public void RegisterPlayerSpawner(PlayerSpawner spawner) => playerSpawns.Add(spawner);
    public void DeregisterPlayerSpawner(PlayerSpawner spawner) => playerSpawns.Remove(spawner);

    public void LoadNextMap() {
        if (mapsIndex < maps.Count) {
            CSceneManager.Instance.LoadScene(maps[mapsIndex]);
            mapsIndex++;
            MovePlayer(.5f);
        } else {
            LogErr("Couldn't load next map, reached end of maps list");
        }
    }

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
}
