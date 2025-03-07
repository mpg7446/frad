using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static MovementInputs inputs;
    public static InputManager Instance;
    public Vector2 movement;


    void Start()
    {
        Instance = this;

        // Register Keybinds
        inputs.Default.Directional.performed += MovementPerformed;
        inputs.Default.Directional.canceled += MovementCanceled;

        inputs.Default.CycleCamera.performed += CycleCameraPerformed;
        inputs.Default.CycleCamera.canceled += CycleCameraCanceled;
    }

    void Update()
    {
        
    }

    private void MovementPerformed(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }

    private void MovementCanceled(InputAction.CallbackContext ctx)
    {
        movement = Vector2.zero;
    }

    public event EventHandler CycleCamera;
    private void CycleCameraPerformed(InputAction.CallbackContext ctx)
    {
        // I dont know how to create an event hook :sob:
    }

    private void CycleCameraCanceled(InputAction.CallbackContext ctx)
    {

    }

}
