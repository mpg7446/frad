using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CryptidUtils
{
    // Objects
    [Header("Objects / Classes")]
    public GameObject cam;
    public GameObject console;
    public GameObject Flashlight;
    private Rigidbody rb;
    public static PlayerManager Instance;
    [Space(10)]

    // settings
    [Header("Settings")]
    public float sensitivity = 1.2f;
    public float speed = 100;
    [Range(0, 90)] public float maxPitch;
    [Range(0, 90)] public float minPitch;
    [Range(0, 90)] public float maxFreeLook;
    private Vector3 movement;

    // Camera
    public bool lockMovement = false;
    private bool freeLooking = false;
    private float pitch;
    private float yaw;
    private float freeYaw;
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

        // Calculate View Transform from Mouse Movement
        pitch -= Input.GetAxis("Mouse Y") * (lockMovement ? sensitivity / 2 : sensitivity);
        pitch = Mathf.Clamp(pitch, -maxPitch, minPitch);

        yaw += Input.GetAxis("Mouse X") * (lockMovement ? sensitivity / 2 : sensitivity);
        if (freeLooking)
        {
            freeYaw += Input.GetAxis("Mouse X") * (lockMovement ? sensitivity / 2 : sensitivity);
            freeYaw = Mathf.Clamp(freeYaw, -maxFreeLook, maxFreeLook);
        } 

        // Apply View Transform
        cam.transform.localEulerAngles = new Vector3(pitch, freeYaw, 0);
        if (!freeLooking)
            transform.eulerAngles = new Vector3(0, yaw, 0);

        // Update Movement Input from InputManager
        if (!lockMovement)
            movement = InputManager.Instance.movement;
    }
    private void FixedUpdate()
    {
        // Escape if fixed in camera view
        if (lockMovement)
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
        lockMovement = true;
        EnableFreeLook();
        Flashlight.SetActive(false);
    }
    public void HideConsole()
    {
        //GameManager.LockCursor();
        CurrentStance = Stance.None;
        console.SetActive(false);
        lockMovement = false;
        DisableFreeLook();
        ConsoleManager.Instance.Spotlight.SetActive(false);
        Flashlight.SetActive(true);
    }

    public void EnableFreeLook()
    {
        freeLooking = true;
    }
    public void DisableFreeLook()
    {
        if (!lockMovement)
        {
            freeLooking = false;
            yaw -= freeYaw;
            freeYaw = 0;
        }
    }
}
