using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DISCLAIMER
// THIS FILE IS NOT IN USE, PLEASE DO NOT USE THIS
public class Console : CryptidUtils
{
    private readonly GameObject Obj;
    private readonly PlayerManager Controller;
    private readonly ConsoleManager Manager;
    private bool IsActive { get
        {
            return Obj.activeSelf;
        } 
    }

    public Console(GameObject obj, PlayerManager controller)
    {
        Obj = obj;
        Controller = controller;
        Manager = ConsoleManager.Instance;
    }

    public void ToggleConsole()
    {
        if (IsActive)
            HideConsole();
        else
            ShowConsole();
    }
    private void HideConsole()
    {
        Obj.SetActive(false);
        Controller.CurrentStance = PlayerManager.Stance.None;
        Controller.lockMovement = false;
        Controller.Flashlight.SetActive(true);
        Manager.Spotlight.SetActive(false);
    }
    private void ShowConsole()
    {
        Obj.SetActive(true);
        Controller.CurrentStance = PlayerManager.Stance.Console;
        Controller.lockMovement = true;
        Controller.Flashlight.SetActive(false);
        Manager.Spotlight.SetActive(true);
        StartCoroutine(Manager.toStaticScreen(0.4f));
    }
}
