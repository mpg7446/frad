using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableCamera : CryptidUtils
{
    private void Start() {
        if (ConsoleManager.Instance == null)
            LogErr("Failed to register camera, no ConsoleManager in scene!");
        else
            ConsoleManager.Instance.RegisterCamera(gameObject);
    }

    private void OnDestroy() => ConsoleManager.Instance.DeregisterCamera(gameObject);
}
