using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : CryptidUtils
{
    public static GameManager Instance;

    public GameObject player;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");
        if (!SceneManager.GetSceneByName("temple run 4").IsValid())
            SceneManager.LoadScene("temple run 4", LoadSceneMode.Additive);
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
