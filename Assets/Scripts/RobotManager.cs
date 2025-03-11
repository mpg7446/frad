using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : CryptidUtils
{
    public static RobotManager Instance;
    public float sensitivity = 1.2f;
    public int speed = 100;
    private Vector3 lookAngle = new Vector3();
    private float pitch;
    private float yaw;
    public bool lockCamera = false;
    public GameObject cam;
    private void Start()
    {
        Instance = this;
        if (cam == null)
            cam = transform.Find("Cam").gameObject;

        // Set Cursor State
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (lockCamera)
            return;

        pitch -= Input.GetAxis("Mouse Y");
        yaw += Input.GetAxis("Mouse X");
        lookAngle = new Vector3(pitch, yaw, 0) * sensitivity;

        if (lookAngle.x >= 90)
            lookAngle.x = 90;
        else if (lookAngle.x <= -90)
            lookAngle.x = -90;

        cam.transform.eulerAngles = lookAngle;
        transform.eulerAngles = new Vector3(0, lookAngle.y, 0);
    }
    private void FixedUpdate()
    {
        if (lockCamera)
            return;

        // pane
        Vector3 movement = Vector3.ClampMagnitude(InputManager.Instance.movement,1);
        transform.Translate(movement * Time.fixedDeltaTime * speed, Space.World);
    }
    private void OnDestroy()
    {
        // Set Cursor State
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
