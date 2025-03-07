using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] private GameObject cam;
    public GameObject screen;
    public RenderTexture screenTexture;
    public RenderTexture switchingTexture;
    public GameObject robot;
    [SerializeField] private GameObject[] cameras;
    [SerializeField] private int selectedCamera = 0;
    [SerializeField] private bool viewingRobot = false;

    void Start()
    {
        // Setup
        Instance = this;
        if (cam == null)
            cam = this.gameObject;
        if (cameras == null)
            Debug.LogError("CameraManager: No cameras provided!");

        transformCamera(cameras[selectedCamera].transform);
    }

    public void CycleCamera()
    {
        selectedCamera++;
        if (selectedCamera >= cameras.Length)
            selectedCamera = 0;

        transformCamera(cameras[selectedCamera].transform);
    }
    public void RobotView()
    {
        viewingRobot = !viewingRobot;
        if (viewingRobot)
            transformCamera(robot.transform);
        else
            transformCamera(cameras[selectedCamera].transform);
    }

    private void transformCamera(Transform transform)
    {
        viewingRobot = false;
        cam.transform.SetParent(transform);
        cam.transform.localPosition = transform.localPosition;
        cam.transform.localRotation = transform.localRotation;
    }


}