using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleManager : CryptidUtils {
    // Objects
    public static ConsoleManager Instance;
    [SerializeField] private GameObject cam;
    private Camera camRenderer;
    [SerializeField] private GameObject screen;
    public GameObject Spotlight;

    // Materials
    [SerializeField] private Material screenMat;
    [SerializeField] private Texture screenTexture;
    [SerializeField] private Texture switchingTexture;

    // Cameras
    [SerializeField] private List<GameObject> cameras;
    [SerializeField] private int selectedCamera = 0;
    private int cameraCooldown;
    public bool CanRender { get {  return cam != null && camRenderer != null && cameraCooldown <= 0; } }
    public bool CanSpotlight { get { return Spotlight != null; } }

    private void Start() {
        // Setup
        Instance = this;
        screenMat = screen.GetComponent<Renderer>().material;
        try {
            screenTexture = screenMat.GetTexture("_EmissionMap");
        } catch {
            LogWarn("Could not find current screen render texture, is it missing?");
        }

        if (cam == null)
            cam = gameObject;
        if (cameras == null)
            LogErr("No cameras provided!");
    }
    private void FixedUpdate() {
        if (cameraCooldown > 0)
            cameraCooldown--;
    }

    #region Camera / Renderer Registration Handling
    public void RegisterCameraRenderer(GameObject renderer) {
        cam = renderer;
        Spotlight = cam.transform.Find("Spot Light").gameObject;
        camRenderer = cam.GetComponent<Camera>();
        TransformCamera(cameras[selectedCamera].transform, false);
    }
    public void DeregisterCameraRenderer(GameObject renderer) {
        cam = null;
        Spotlight = null;
        camRenderer = null;
    }

    public void RegisterCamera(GameObject camera) => cameras.Add(camera);
    public void DeregisterCamera(GameObject camera) => cameras.Remove(camera);
    #endregion

    public void CycleCamera() {
        if (!CanRender || !PlayerManager.Instance.lockMovement)
            return;
        cameraCooldown = 30;

        selectedCamera++;
        if (selectedCamera >= cameras.Count)
            selectedCamera = 0;

        TransformCamera(cameras[selectedCamera].transform);
    }

    // System no longer in use
    //public void RobotView()
    //{
    //    if (cooldown > 0)
    //        return;
    //    cooldown = 30;

    //    viewingRobot = !viewingRobot;
    //    if (viewingRobot)
    //    {
    //        transformCamera(player.transform);
    //        PlayerManager.Instance.lockCamera = false;
    //    }
    //    else
    //    {
    //        transformCamera(cameras[selectedCamera].transform);
    //        PlayerManager.Instance.lockCamera = true;
    //    }
    //}

    private void TransformCamera(Transform transform, bool light = true) {
        StartCoroutine(ToStaticScreen(0.6f, light));

        cam.transform.SetParent(transform);
        cam.transform.SetPositionAndRotation(transform.position, transform.rotation);

        if (light)
            Spotlight.SetActive(false);
        camRenderer.Render();
    }

    public IEnumerator ToStaticScreen(float time, bool light = true) {
        //Log("pissing myself rn (attempting to show static screen)");
        try {
            screenMat.SetTexture("_EmissionMap",switchingTexture);
        } catch {
            LogWarn("Failed to access switchingTexture, is it missing?");
        }

        yield return new WaitForSeconds(time);

        if (PlayerManager.Instance.CurrentStance == PlayerManager.Stance.Console)
            Spotlight.SetActive(light);
        screenMat.SetTexture("_EmissionMap", screenTexture);
    }
}