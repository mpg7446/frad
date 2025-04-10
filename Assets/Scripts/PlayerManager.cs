using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerManager : CryptidUtils
{
    // Objects
    [Header("Objects / Classes")]
    public GameObject cam;
    public GameObject console;
    public GameObject Flashlight;
    //private Rigidbody rb;
    private NavMeshAgent agent;
    public static PlayerManager Instance;

    // settings
    [Space(10)]
    [Header("Settings")]
    public float sensitivity = 1.2f;
    [Range(0, 90)]
    public float maxPitch = 80;
    [Range(0, 90)] public float minPitch = 31.1f;
    //[Range(0, 90)] 
    public float maxFreeLook = 56;
    private float lockSmoothing = 1;

    // movement
    [Space(10)]
    [Header("Movement")]
    public float speed = 1;
    public float sprintingSpeed = 2;
    [Tooltip("Maximum sprint in seconds")]
    public float maxSprint = 3;
    private bool sprinting;
    private float sprint;
    private Vector3 movement;

    // Camera
    [Space(10)]
    [Header("Camera")]
    public PostProcessVolume postProcessing;
    private Vignette vignette;
    public float maxVignette;
    public bool lockMovement = false;
    //private bool freeLooking = false;
    private float pitch;
    private float yaw;
    private float freeYaw;

    // Animation Stance
    [Space(10f)]
    [Header("Animation")]
    public Stance CurrentStance;
    public enum Stance
    {
        None,
        Console
    }
    public float Step { get; private set; }
    private void Start()
    {
        Instance = this;
        if (cam == null)
            cam = transform.Find("Cam").gameObject;
        
        // Post Processing Vignette
        if (postProcessing == null)
            postProcessing = cam.GetComponent<PostProcessVolume>();
        postProcessing.profile.TryGetSettings(out vignette);

        // Movement
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        GameManager.LockCursor();
    }
    private void OnDestroy()
    {
        GameManager.UnlockCursor();
    }
    private void Update()
    {
        ApplyRotation();

        // Apply sprinting vignette
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, sprinting ? maxVignette : 0, Time.deltaTime / maxSprint);

        // Update Movement Input from InputManager
        if (!lockMovement)
        {
            movement = InputManager.Instance.movement;
            sprinting = InputManager.Instance.sprinting;
            //step += (Mathf.Abs(movement.x) + Mathf.Abs(movement.z)) / (speed + rb.mass);
            Step += (Mathf.Abs(movement.x) + Mathf.Abs(movement.z)) / agent.speed;
        } else if (movement != Vector3.zero)
        {
            movement = Vector3.zero;
            sprinting = false;
        }
    }
    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyRotation()
    {
        // Calculate View Transform from Mouse Movement
        pitch -= Input.GetAxis("Mouse Y") * (lockMovement ? sensitivity / 2 : sensitivity);
        pitch = Mathf.Clamp(pitch, -maxPitch, minPitch);

        lockSmoothing = Mathf.Lerp(lockSmoothing, lockMovement ? 0 : 1, Time.deltaTime / 0.12f);

        freeYaw += Input.GetAxis("Mouse X") * (lockMovement ? sensitivity / 2 : sensitivity);
        yaw = Mathf.Lerp(yaw, yaw + ((freeYaw - yaw) * lockSmoothing), Time.deltaTime);
        //yaw = Mathf.MoveTowards(yaw, yaw + ((freeYaw - yaw) * lockSmoothing), 0.2f);
        freeYaw = Mathf.Clamp(freeYaw, yaw - maxFreeLook, yaw + maxFreeLook);

        // Apply View Transform
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        cam.transform.localRotation = Quaternion.Euler(pitch, freeYaw - yaw, 0);
    }
    private void ApplyMovement()
    {
        // Escape if fixed in camera view
        if (lockMovement)
            return;

        // movement speed
        if (sprinting)
        {
            if (sprint < maxSprint)
            {
                agent.speed = sprintingSpeed;
                sprint += Time.fixedDeltaTime;
            } else
                agent.speed = speed;
        } else
        {
            agent.speed = speed;
            sprint = Mathf.Clamp(sprint - Time.fixedDeltaTime, 0, maxSprint);
        }

            // pane
            Vector3 moveForce = ((cam.transform.forward + transform.forward) / 2) * movement.z + ((cam.transform.right + transform.right) / 2) * movement.x;
        //rb.AddForce(moveForce.normalized * speed * 100, ForceMode.Force);
        if (moveForce != Vector3.zero)
            agent.SetDestination(transform.position + moveForce);
    }

    #region Console
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
        //EnableFreeLook();
        Flashlight.SetActive(false);
    }
    public void HideConsole()
    {
        //GameManager.LockCursor();
        CurrentStance = Stance.None;
        console.SetActive(false);
        lockMovement = false;
        //DisableFreeLook();
        ConsoleManager.Instance.Spotlight.SetActive(false);
        Flashlight.SetActive(true);
    }
    #endregion
}
