using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : CryptidUtils
{
    public static CameraManager Instance;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject screen;
    [SerializeField] private Material screenMat;
    public Texture screenTexture { get; private set; }
    public Texture switchingTexture;
    public GameObject robot;
    [SerializeField] private GameObject[] cameras;
    [SerializeField] private int selectedCamera = 0;
    [SerializeField] private bool viewingRobot = false;
    private int cooldown;

    private void Start()
    {
        // Setup
        Instance = this;
        screenMat = screen.GetComponent<Renderer>().material;
        try {
            screenTexture = screenMat.GetTexture("_EmissionMap");
        } catch {
            LogWarning("Could not find current screen render texture, is it missing?");
        }

        if (cam == null)
            cam = gameObject;
        if (cameras == null)
            LogError("No cameras provided!");

        transformCamera(cameras[selectedCamera].transform);
        RobotManager.Instance.lockCamera = true;
    }
    private void FixedUpdate()
    {
        if (cooldown > 0)
            cooldown--;
    }

    public void CycleCamera()
    {
        if (cooldown > 0)
            return;
        cooldown = 30;

        selectedCamera++;
        if (selectedCamera >= cameras.Length)
            selectedCamera = 0;

        transformCamera(cameras[selectedCamera].transform);
    }
    public void RobotView()
    {
        if (cooldown > 0)
            return;
        cooldown = 30;

        viewingRobot = !viewingRobot;
        if (viewingRobot)
        {
            transformCamera(robot.transform);
            RobotManager.Instance.lockCamera = false;
        }
        else
        {
            transformCamera(cameras[selectedCamera].transform);
            RobotManager.Instance.lockCamera = true;
        }
    }

    private void transformCamera(Transform transform)
    {
        StartCoroutine(toStaticScreen(0.6f));
        viewingRobot = false;
        cam.transform.SetParent(transform);
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }

    private IEnumerator toStaticScreen(float time)
    {
        //Log("pissing myself rn (attempting to show static screen)");
        try {
            screenMat.SetTexture("_EmissionMap",switchingTexture);
        } catch {
            LogWarning("Failed to access switchingTexture, is it missing?");
        }

        yield return new WaitForSeconds(time);
        screenMat.SetTexture("_EmissionMap", screenTexture);
    }

}