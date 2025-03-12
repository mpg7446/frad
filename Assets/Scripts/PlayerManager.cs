using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CryptidUtils
{
    // Objects
    [Header("Objects / Classes")]
    public GameObject cam;
    public GameObject console;
    private Rigidbody rb;
    public static PlayerManager Instance;
    [Space(10)]

    // settings
    [Header("Settings")]
    public float sensitivity = 1.2f;
    public int speed = 100;
    [Range(0, 90)]
    public float maxPitch;
    [Range(0f, 90)]
    public float minPitch;
    private Vector3 movement;

    // Camera
    public bool lockCamera = false;
    private Vector3 lookAngle = new Vector3();
    private float pitch;
    private float yaw;
    [Space(10f)]

    // Animation Stance
    [Header("Animation")]
    public Stance CurrentStance;
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
        pitch = Mathf.Clamp(pitch, -maxPitch, minPitch);
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
    public void ShowConsole()
    {
        //GameManager.UnlockCursor();
        CurrentStance = Stance.Console;
        console.SetActive(true);
        StartCoroutine(ConsoleManager.Instance.toStaticScreen(0.4f));
        lockCamera = true;
    }
    public void HideConsole()
    {
        //GameManager.LockCursor();
        CurrentStance = Stance.None;
        console.SetActive(false);
        lockCamera = false;
    }
}
