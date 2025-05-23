using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : CryptidUtils {
    public static InputManager Instance;
    private static MovementInputs inputs;
    public Vector3 movement;
    public bool sprinting;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");
        inputs = new MovementInputs();

        // Register Keybinds
        inputs.Default.Directional.performed += DirectionalPerformed;
        inputs.Default.Directional.canceled += DirectionalCanceled;

        inputs.Default.Pause.performed += PausePerformed;
        inputs.Default.Pause.canceled += PauseCanceled;

        inputs.Default.Console.performed += ConsonlePerformed;
        inputs.Default.Console.canceled += ConsoleCanceled;

        inputs.Default.Interact.performed += InteractPerformed;
        inputs.Default.Interact.canceled += InteractCanceled;

        inputs.Default.Sprint.performed += SprintPerformed;
        inputs.Default.Sprint.canceled += SprintCanceled;

        inputs.Enable();
    }
    private void OnDestroy() => inputs.Disable();
    private void OnDisable() => inputs.Disable();

    private void DirectionalPerformed(InputAction.CallbackContext ctx) { 
        Vector2 mv = ctx.ReadValue<Vector2>();
        movement = new Vector3(mv.x, 0, mv.y);
    }
    private void DirectionalCanceled(InputAction.CallbackContext ctx) => movement = Vector3.zero;

    private void PausePerformed(InputAction.CallbackContext ctx) { 
        if (!GameManager.Instance.isPaused) {
            GameManager.Instance.Pause();
            Director.Instance.Pause();
        } else {
            GameManager.Instance.Play();
            Director.Instance.Play();
        }
    }
    private void PauseCanceled(InputAction.CallbackContext ctx) { }

    private void ConsonlePerformed(InputAction.CallbackContext ctx) => PlayerManager.Instance.ToggleConsole();
    private void ConsoleCanceled(InputAction.CallbackContext ctx) { }

    private void InteractPerformed(InputAction.CallbackContext ctx) {
        if (PlayerManager.Instance.CurrentStance == PlayerManager.Stance.None)
            PlayerManager.Instance.Interact();
        else
            ConsoleManager.Instance.CycleCamera();
    }
    private void InteractCanceled(InputAction.CallbackContext ctx) { }

    private void SprintPerformed(InputAction.CallbackContext ctx) => sprinting = true;
    private void SprintCanceled(InputAction.CallbackContext ctx) => sprinting = false;
}
