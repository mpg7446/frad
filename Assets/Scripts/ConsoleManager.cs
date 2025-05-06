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

    public void RegisterCameraRenderer(GameObject renderer) {
        cam = renderer;
        camRenderer = cam.GetComponent<Camera>();
        TransformCamera(cameras[selectedCamera].transform, false);
    }
    public void RegisterCamera(GameObject camera) => cameras.Add(camera);

    public void CycleCamera() {
        if (cameraCooldown > 0 || !PlayerManager.Instance.lockMovement)
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
        Spotlight.SetActive(false);
        StartCoroutine(ToStaticScreen(0.6f));
        //viewingRobot = false;
        cam.transform.SetParent(transform, light);
        cam.transform.SetPositionAndRotation(transform.position, transform.rotation);
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
        Spotlight.SetActive(light);
        screenMat.SetTexture("_EmissionMap", screenTexture);
    }
}