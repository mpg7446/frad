using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    public enum Menu {
        None,
        Main,
        Pause,
        Settings,
        Rewards,
        Overlay
    }

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject rewardsMenu;
    [SerializeField] private GameObject overlay;

    private List<GameObject> _menusList = new(); // internal list of existing menus, used for closing open menus
    private Dictionary<Menu, GameObject> _menus = new(); // internal dictionary list of existing menus for quick access to menu GameObject using Menu enum

    private void Start() {
        Instance = this;
        SetValues();
        CloseMenus();
    }

    private void SetValues() {
        _menusList = new() {
            mainMenu,
            pauseMenu,
            settingsMenu,
            rewardsMenu,
            overlay
        };
        _menus = new() {
            { Menu.Main, mainMenu },
            { Menu.Pause, pauseMenu },
            { Menu.Settings, settingsMenu },
            { Menu.Rewards, rewardsMenu },
            { Menu.Overlay, overlay }
        };
    }

    private void OpenMenu(Menu menu, bool exclusive = true) {
        // close existing menus
        if (exclusive)
            CloseMenus();

        // open selected menu
        _menus[menu].SetActive(true);
    }

    private void CloseMenus() {
        foreach (GameObject obj in _menusList)
            obj.SetActive(false);
    }

    public void OpenMain() => OpenMenu(Menu.Main, true);
    public void OpenPause() => OpenMenu(Menu.Pause, true);
    public void OpenSettings() => OpenMenu(Menu.Settings, true);
    public void OpenRewards() => OpenMenu(Menu.Rewards, true);
    public void OpenOverlay() => OpenMenu(Menu.Overlay, true);
}
