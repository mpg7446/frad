using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private static MovementInputs inputs;
    public Vector2 movement;

    void Start()
    {
        Instance = this;
        inputs = new MovementInputs();

        // Register Keybinds
        inputs.Default.Directional.performed += DirectionalPerformed;
        inputs.Default.Directional.canceled += DirectionalCanceled;

        inputs.Default.CycleCamera.performed += CycleCameraPerformed;
        inputs.Default.CycleCamera.canceled += CycleCameraCanceled;

        inputs.Default.Robot.performed += RobotPerformed;
        inputs.Default.Robot.canceled += RobotCanceled;

        inputs.Enable();
    }

    private void DirectionalPerformed(InputAction.CallbackContext ctx) { movement = ctx.ReadValue<Vector2>(); }
    private void DirectionalCanceled(InputAction.CallbackContext ctx) { movement = Vector2.zero; }
    private void CycleCameraPerformed(InputAction.CallbackContext ctx) { CameraManager.Instance.CycleCamera(); }
    private void CycleCameraCanceled(InputAction.CallbackContext ctx) { }
    private void RobotPerformed(InputAction.CallbackContext ctx) { CameraManager.Instance.RobotView(); }
    private void RobotCanceled(InputAction.CallbackContext ctx) { }

}
