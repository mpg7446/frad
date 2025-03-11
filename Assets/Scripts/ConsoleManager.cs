using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleManager : CryptidUtils
{
    // Objects
    public static ConsoleManager Instance;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject screen;

    // Materials
    [SerializeField] private Material screenMat;
    [SerializeField] private Texture screenTexture;
    [SerializeField] private Texture switchingTexture;

    // Cameras
    [SerializeField] private GameObject[] cameras;
    [SerializeField] private int selectedCamera = 0;
    private int cameraCooldown;

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
    }
    private void FixedUpdate()
    {
        if (cameraCooldown > 0)
            cameraCooldown--;
    }

    public void CycleCamera()
    {
        if (cameraCooldown > 0)
            return;
        cameraCooldown = 30;

        selectedCamera++;
        if (selectedCamera >= cameras.Length)
            selectedCamera = 0;

        transformCamera(cameras[selectedCamera].transform);
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

    private void transformCamera(Transform transform)
    {
        StartCoroutine(toStaticScreen(0.6f));
        //viewingRobot = false;
        cam.transform.SetParent(transform);
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }

    public IEnumerator toStaticScreen(float time)
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