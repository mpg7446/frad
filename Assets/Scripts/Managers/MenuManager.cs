using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    public enum Menu {
        None,
        Pause,
        Settings
    }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private List<GameObject> _menusList = new(); // internal list of existing menus, used for closing open menus
    private Dictionary<Menu, GameObject> _menus = new(); // internal dictionary list of existing menus for quick access to menu GameObject using Menu enum

    private void Start() {
        Instance = this;
        SetValues();
        CloseMenus();
    }

    public void SetValues() {
        _menusList = new() {
            pauseMenu,
            settingsMenu
        };
        _menus = new() {
            { Menu.Pause, pauseMenu },
            { Menu.Settings, settingsMenu }
        };
    }

    public void OpenMenu(Menu menu, bool exclusive = true) {
        // close existing menus
        if (exclusive)
            CloseMenus();

        // open selected menu
        _menus[menu].SetActive(true);
    }

    public void CloseMenus() {
        foreach (GameObject obj in _menusList)
            obj.SetActive(false);
    }

    public void OpenPause() => OpenMenu(Menu.Pause, true);
    public void OpenSettings() => OpenMenu(Menu.Settings, true);
}
