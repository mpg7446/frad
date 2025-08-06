using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    public enum Menu {
        Save,
        Main,
        Pause,
        Settings,
        Rewards,
        Overlay
    }

    [SerializeField] private GameObject menuCamera;

    [Space]
    [Header("Menus")]
    [SerializeField] private GameObject saveMenu;
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

    // moved to seperate function in case in needs to be called again - shouldnt need to tho
    private void SetValues() {
        _menusList = new() {
            saveMenu,
            mainMenu,
            pauseMenu,
            settingsMenu,
            rewardsMenu,
            overlay
        };
        _menus = new() {
            { Menu.Save, saveMenu },
            { Menu.Main, mainMenu },
            { Menu.Pause, pauseMenu },
            { Menu.Settings, settingsMenu },
            { Menu.Rewards, rewardsMenu },
            { Menu.Overlay, overlay }
        };
    }

    private void OpenMenu(Menu menu, bool exclusive = true, bool requireCam = true) {
        // close existing menus
        if (exclusive)
            CloseMenus();

        // open selected menu
        _menus[menu].SetActive(true);

        // set camera visibility for menus that require it
        menuCamera.SetActive(requireCam);
    }

    private void CloseMenus() {
        foreach (GameObject obj in _menusList)
            obj.SetActive(false);
    }

    // button callbacks - information could be set in a different class (similar to saves/settings)
    public void OpenSaves() => OpenMenu(Menu.Save, true);
    public void OpenMain() => OpenMenu(Menu.Main, true);
    public void OpenPause() => OpenMenu(Menu.Pause, true, false);
    public void OpenSettings() => OpenMenu(Menu.Settings, true, false);
    public void OpenRewards() => OpenMenu(Menu.Rewards, true);
    public void OpenOverlay() => OpenMenu(Menu.Overlay, true, false);
}
