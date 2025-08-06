using PSXShaderKit;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerManager : CryptidUtils {
    public static PlayerManager Instance;

    // Objects
    [Header("Objects / Classes")]
    public GameObject cam;
    public GameObject console;
    public GameObject Flashlight;
    public NavMeshAgent agent;

    // settings
    [Space]
    [Header("Settings")]
    public float sensitivity = 1.2f;
    [Range(0, 90)]
    public float maxPitch = 80;
    [Range(0, 90)] 
    public float minPitch = 31.1f;
    [Range(0, 90)] 
    public float maxFreeLook = 56;
    private float lockSmoothing = 1;
    [Range(0,2)] public float pixelationIntensity = 1.534f; // min (0) = 1 | max (2) = 0.01
    [Range(0,2)] public float ditheringScale = 0;

    // movement
    [Space]
    [Header("Movement")]
    public float speed = 1;
    public float sprintMod = 2;
    [Tooltip("Maximum sprint in seconds")]
    public float maxSprint = 3;
    private bool sprinting;
    private float sprint;
    private Vector3 movement;
    public Room room;

    // Camera
    [Space]
    [Header("Camera / Rendering")]
    public PostProcessVolume postProcessing;
    private Vignette vignette;
    public float maxVignette = 1;
    public PSXPostProcessEffect PSXPostProcessing;
    public bool lockMovement = false;
    public bool lockCamera = false;
    private float pitch;
    private float yaw;
    private float freeYaw;

    // Interactions
    [Space]
    [Header("Interactions")]
    public InteractableObject lookingAt;
    private Locker locker = null;
    public bool InLocker { get; private set; }
    private float oxygen = 100;
    [Tooltip("Time (in seconds) it takes for 1/100th of the players oxygen to deplete per damage taken (or how long to drain at full damagetaken)")]
    [SerializeField] private float oxygenDrainRate = 1f;
    private float health = 100;
    private float damageTaken = 0;
    public int score = 0;

    // Animation Stance
    [Space]
    [Header("Animation")]
    public Stance CurrentStance;
    public enum Stance {
        None,
        Console
    }
    public float Step { get; private set; }

    private void Start() {
        Instance = this;
        if (cam == null)
            cam = transform.Find("Cam").gameObject;
        
        // Post Processing
        if (postProcessing == null)
            postProcessing = cam.GetComponent<PostProcessVolume>();
        postProcessing.profile.TryGetSettings(out vignette);
        if (PSXPostProcessing == null)
            PSXPostProcessing = cam.GetComponent<PSXPostProcessEffect>();

        // Movement
        agent = GetComponent<NavMeshAgent>();
        GameManager.LockCursor();
        InLocker = false;
        
        //for (int i = 0; i < InventoryManager.Instance.Inventory.Length; i++) {
        //    if (InventoryManager.Instance.Inventory[i] != null)
        //        ModMovement(InventoryManager.Instance.Inventory[i]);
        //}

        foreach (ScriptableItem item in InventoryManager.Instance.Inventory) {
            if (item != null) 
                ModMovement(item);
        }
    }
    private void OnDestroy() => GameManager.UnlockCursor();
    private void Update() {
        if (!lockCamera)
            ApplyRotation();

        // Apply sprinting vignette
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, sprinting ? maxVignette : 0, Time.fixedDeltaTime / (maxSprint * maxSprint));
        //if (sprinting)
        //    Log($"vignette value : ({vignette.intensity.value}/{maxVignette}) ({sprint}/{maxSprint})");

        // Update Movement Input from InputManager
        if (!lockMovement) {
            movement = InputManager.Instance.movement;
            sprinting = InputManager.Instance.sprinting;
            Step += (Mathf.Abs(movement.x) + Mathf.Abs(movement.z)) / agent.speed;
        } else if (movement != Vector3.zero) {
            movement = Vector3.zero;
            sprinting = false;
        }

        if (GameManager.Instance.isPaused)
            return;

        if (!GameManager.Instance.isPaused && lookingAt != null && Vector3.Distance(cam.transform.position, lookingAt.transform.position) < lookingAt.maxDistance)
            GameManager.ShowPseudoCursor();
        else
            GameManager.HidePseudoCursor();
    }
    private void FixedUpdate() {
        ApplyMovement();

        if (damageTaken > 0)
            UpdateOxygen();
    }

    private void OnValidate() {
        UpdateGraphics();
        if (SettingsManager.hasChanged)
            UpdateSettings();
    }

    public void Pause() {
        lockMovement = true;
        lockCamera = true;
        agent.enabled = false;
    }

    public void Play() {
        lockMovement = false;
        lockCamera = false;
        agent.enabled = true;
    }

    #region Settings and Rendering
    // TODO please finish this qwq
    public void UpdateSettings() {
        sensitivity = SettingsManager.s_sensitivity;
        maxVignette = SettingsManager.s_maxVignette;
        pixelationIntensity = SettingsManager.s_pixelationIntensity;
        ditheringScale = SettingsManager.s_ditheringScale;
    }

    public void UpdateGraphics() {
        float pixelValue = (2 - pixelationIntensity) / 2;
        PSXPostProcessing._PixelationFactor = (float)Math.Clamp(pixelValue, 0.01, 1);
        float ditherValue = (2 - ditheringScale) / 2;
        PSXPostProcessing._DitheringScale = (float)Math.Clamp(ditherValue, 0.1, 1);
        PSXPostProcessing.UpdateValues();
    }
    #endregion

    #region Movement
    private void ApplyRotation() {
        // Calculate View Transform from Mouse Movement
        pitch -= Input.GetAxis("Mouse Y") * (lockMovement ? sensitivity / 2 : sensitivity);
        pitch = Mathf.Clamp(pitch, -maxPitch, minPitch);

        lockSmoothing = Mathf.Lerp(lockSmoothing, lockMovement ? 0 : 1, Time.deltaTime / 0.12f);

        freeYaw += Input.GetAxis("Mouse X") * (lockMovement ? sensitivity / 2 : sensitivity);
        yaw = Mathf.Lerp(yaw, yaw + ((freeYaw - yaw) * lockSmoothing), Time.deltaTime);
        freeYaw = Mathf.Clamp(freeYaw, yaw - maxFreeLook, yaw + maxFreeLook);

        // Apply View Transform
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        cam.transform.localRotation = Quaternion.Euler(pitch, freeYaw - yaw, 0);
    }
    private void ApplyMovement() {
        // Escape if fixed in camera view
        if (lockMovement)
            return;

        // movement speed
        if (sprinting) {
            if (sprint < maxSprint) 
                sprint += Time.fixedDeltaTime;
        } else {
            sprint = Mathf.Clamp(sprint - Time.fixedDeltaTime, 0, maxSprint);
        }
        agent.speed = sprinting ? speed * sprintMod : speed;

        // pane
        Vector3 moveForce = ((cam.transform.forward + transform.forward) / 2) * movement.z + ((cam.transform.right + transform.right) / 2) * movement.x;
        //rb.AddForce(moveForce.normalized * speed * 100, ForceMode.Force);
        if (moveForce != Vector3.zero)
            agent.SetDestination(transform.position + moveForce);
    }

    public void ModMovement(ScriptableItem item) {
        speed *= item.speedMultiplier;
        sprintMod *= item.sprintSpeedMultiplier;
        maxSprint *= item.sprintDurationMultiplier;
    }

    public void ForceMoveTo(Vector3 position, Quaternion rotation) {
        agent.ResetPath();
        agent.enabled = false;
        yaw = rotation.eulerAngles.y;
        freeYaw = yaw;
        transform.SetPositionAndRotation(position, rotation);
        agent.enabled = true;
    }
    public void ForceMoveTo(Transform transform) => ForceMoveTo(transform.position, transform.rotation);
    #endregion

    #region Console
    public void ToggleConsole() {
        if (console == null || InLocker)
            return;

        switch(CurrentStance) {
            case Stance.None:
                ShowConsole();
                break;
            case Stance.Console:
                HideConsole();
                break;
        }
    }
    public void ShowConsole() { // a lot of these functions will be replaced with animations instead later down the line
        //GameManager.UnlockCursor();
        CurrentStance = Stance.Console;
        console.SetActive(true);
        StartCoroutine(ConsoleManager.Instance.ToStaticScreen(0.4f));
        lockMovement = true;
        //EnableFreeLook();
        Flashlight.SetActive(false);
    }
    public void HideConsole() {
        //GameManager.LockCursor();
        CurrentStance = Stance.None;
        console.SetActive(false);
        lockMovement = false;
        //DisableFreeLook();
        //ConsoleManager.Instance.Spotlight.SetActive(false); - not sure why this is here, flashlight should be handled by PlayerManager
        Flashlight.SetActive(true);
    }

    #endregion

    #region Interactions
    public void Interact() {
        // already in locker, no other interactions possible
        if (InLocker) {
            ExitLocker();
            return;
        }

        // no interactable object is found
        if (lookingAt == null)
            return;

        // attempt interact with object, escape if not in reach / cant be interacted with
        if (!lookingAt.Interact(gameObject))
            return;

        if (lookingAt is Locker targetLocker) {
            EnterLocker(targetLocker);
            return;
        }
    }

    private void EnterLocker(Locker targetLocker) {
        // set agent and internal locker/movement state
        agent.ResetPath();
        agent.enabled = false;
        InLocker = true;
        lockMovement = true;
        locker = targetLocker;

        // set rotation & position
        yaw = locker.transform.eulerAngles.y;
        freeYaw = locker.transform.eulerAngles.y;
        transform.SetPositionAndRotation(locker.transform.position, locker.transform.rotation);
        lockSmoothing = 0;

        // play animation
    }
    private void ExitLocker() {
        // set agent and internal locker/movement state
        agent.enabled = true;
        agent.ResetPath();
        transform.SetPositionAndRotation(locker.exitPosition.transform.position, locker.exitPosition.transform.rotation);
        locker.ExitLocker();

        // set rotation & position
        InLocker = false;
        lockMovement = false;
        locker = null;

        // play animation
    }
    #endregion

    public void Damage(float amount, bool affectHealth = true, bool damageHelmet = true) {
        amount = Mathf.Clamp(amount, 0, 100);
        
        if (damageHelmet)
            damageTaken += amount;

        if (affectHealth)
            health -= amount;

        if (health <= 0)
            GameManager.Instance.StopGame();
    }

    private void UpdateOxygen() {
        float drain = (damageTaken * Time.fixedDeltaTime) / oxygenDrainRate;
        oxygen -= drain;

        if (oxygen <= 0)
            GameManager.Instance.StopGame();
    }
}
