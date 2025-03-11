using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CryptidUtils
{
    // Objects
    public static PlayerManager Instance;
    public GameObject cam;
    public GameObject console;
    private Rigidbody rb;

    // settings
    public float sensitivity = 1.2f;
    public int speed = 100;

    private Vector3 movement;

    // Camera
    private float pitch;
    private float yaw;
    private Vector3 lookAngle = new Vector3();
    public bool lockCamera = false;

    // Animation Stance
    public Stance CurrentStance {  get; private set; }
    public enum Stance
    {
        None,
        Console
    }
    private void Start()
    {
        Instance = this;
        if (cam == null)
            cam = transform.Find("Cam").gameObject;
        rb = this.GetComponent<Rigidbody>();
        GameManager.LockCursor();
    }
    private void OnDestroy()
    {
        GameManager.UnlockCursor();
    }
    private void Update()
    {
        // Escape if fixed in camera view
        if (lockCamera)
            return;

        movement = InputManager.Instance.movement;

        // Calculate View Transform from Mouse Movement
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);
        lookAngle = new Vector3(pitch, yaw, 0);

        // Apply View Transform
        cam.transform.localEulerAngles = new Vector3(lookAngle.x, 0, 0);
        transform.eulerAngles = new Vector3(0, lookAngle.y, 0);
    }
    private void FixedUpdate()
    {
        // Escape if fixed in camera view
        if (lockCamera)
            return;

        // pane
        Vector3 moveForce = transform.forward * movement.z + transform.right * movement.x;
        rb.AddForce(moveForce.normalized * speed * 100, ForceMode.Force);
    }

    // Console
    public void ToggleConsole()
    {
        switch(CurrentStance)
        {
            case Stance.None:
                ShowConsole();
                break;
            case Stance.Console:
                HideConsole();
                break;
        }
    }
    private void ShowConsole()
    {
        GameManager.UnlockCursor();
        CurrentStance = Stance.Console;
        console.SetActive(true);
        StartCoroutine(ConsoleManager.Instance.toStaticScreen(0.4f));
        lockCamera = true;
    }
    private void HideConsole()
    {
        GameManager.LockCursor();
        CurrentStance = Stance.None;
        console.SetActive(false);
        lockCamera = false;
    }
}
