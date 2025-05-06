using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : CryptidUtils {
    public static GameManager Instance;

    public GameObject player;
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

    public void LoadNextMap() {
        if (mapsIndex < maps.Count) {
            CSceneManager.Instance.LoadScene(maps[mapsIndex]);
            mapsIndex++;
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
