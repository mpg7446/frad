using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DISCLAIMER
// THIS FILE IS NOT IN USE, PLEASE DO NOT USE THIS
public class Console : CryptidUtils
{
    private GameObject obj;
    private PlayerManager controller;
    private ConsoleManager manager;
    private bool isActive { get
        {
            return obj.activeSelf;
        } 
    }

    public Console(GameObject obj, PlayerManager controller)
    {
        this.obj = obj;
        this.controller = controller;
        manager = ConsoleManager.Instance;
    }

    public void ToggleConsole()
    {
        if (isActive)
            HideConsole();
        else
            ShowConsole();
    }
    private void HideConsole()
    {
        obj.SetActive(false);
        controller.CurrentStance = PlayerManager.Stance.None;
        controller.lockMovement = false;
        controller.Flashlight.SetActive(true);
        manager.Spotlight.SetActive(false);
    }
    private void ShowConsole()
    {
        obj.SetActive(true);
        controller.CurrentStance = PlayerManager.Stance.Console;
        controller.lockMovement = true;
        controller.Flashlight.SetActive(false);
        manager.Spotlight.SetActive(true);
        StartCoroutine(manager.toStaticScreen(0.4f));
    }
}
