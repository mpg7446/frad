using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CCTVCam : MonoBehaviour
{
    public float fps = 20;
    private float elapsed;
    private Camera cam;
    private bool renderable
    {
        get => (fps > 0 ? elapsed > 1f / fps : elapsed > 1) && random(3);
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (renderable)
        {
            elapsed = 0;
            cam.Render();
        } else
        {
            cam.enabled = false;
        }
    }
    private bool random(int range)
    {
        int rng = UnityEngine.Random.Range(0, range-1);
        elapsed = 0;
        return rng == 0;
    }
}
