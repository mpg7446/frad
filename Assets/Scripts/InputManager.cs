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
    public bool sprinting;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");
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

        inputs.Default.Sprint.performed += SprintPerformed;
        inputs.Default.Sprint.canceled += SprintCanceled;

        inputs.Enable();
    }
    private void OnDestroy() => inputs.Disable();
    private void OnDisable() => inputs.Disable();

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

    private void FreeLookPerformed(InputAction.CallbackContext ctx) { /*Director.Instance.Spawn(Director.ID.Fazball, new Vector3(0, 2, 0));*/ }
    private void FreeLookCanceled(InputAction.CallbackContext ctx) { /*PlayerManager.Instance.DisableFreeLook();*/ }

    private void SprintPerformed(InputAction.CallbackContext ctx) { sprinting = true; }
    private void SprintCanceled(InputAction.CallbackContext ctx) { sprinting = false; }
}
