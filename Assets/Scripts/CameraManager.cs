using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject[] cameras;

    // Start is called before the first frame update
    void Start()
    {
        // Setup
        if (cam == null)
            cam = this.gameObject;
        if (cameras == null)
            Debug.LogError("CameraManager: No cameras provided!");

        // Register Events
        //InputManager.Instance.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
