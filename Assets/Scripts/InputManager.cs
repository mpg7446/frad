using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : CryptidUtils
{
    public static InputManager Instance;
    private static MovementInputs inputs;
    public Vector3 movement;

    void Start()
    {
        Instance = this;
        inputs = new MovementInputs();

        // Register Keybinds
        inputs.Default.Directional.performed += DirectionalPerformed;
        inputs.Default.Directional.canceled += DirectionalCanceled;

        inputs.Default.CycleCamera.performed += CycleCameraPerformed;
        inputs.Default.CycleCamera.canceled += CycleCameraCanceled;

        inputs.Default.Console.performed += ConsonlePerformed;
        inputs.Default.Console.canceled += ConsoleCanceled;

        inputs.Default.FreeLook.performed += FreeLookPerformed;
        inputs.Default.FreeLook.canceled += FreeLookCanceled;

        inputs.Enable();
    }

    private void DirectionalPerformed(InputAction.CallbackContext ctx) 
    { 
        Vector2 mv = ctx.ReadValue<Vector2>();
        movement = new Vector3(mv.x, 0, mv.y);
    }
    private void DirectionalCanceled(InputAction.CallbackContext ctx) { movement = Vector3.zero; }

    private void CycleCameraPerformed(InputAction.CallbackContext ctx) { ConsoleManager.Instance.CycleCamera(); }
    private void CycleCameraCanceled(InputAction.CallbackContext ctx) { }

    private void ConsonlePerformed(InputAction.CallbackContext ctx) { PlayerManager.Instance.ToggleConsole(); }
    private void ConsoleCanceled(InputAction.CallbackContext ctx) { }

    private void FreeLookPerformed(InputAction.CallbackContext ctx) { /*PlayerManager.Instance.EnableFreeLook();*/ }
    private void FreeLookCanceled(InputAction.CallbackContext ctx) { /*PlayerManager.Instance.DisableFreeLook();*/ }

}
