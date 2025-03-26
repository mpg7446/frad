using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerManager : MonoBehaviour
{
    public GameObject[] lights;
    [Range(0, 50)] public float fps;
    private int tick;

    private void FixedUpdate()
    {
        tick++;
        if (tick >= 1f / fps)
        {
            int index = UnityEngine.Random.Range(0, lights.Length + 1);
            if (lights[index].activeSelf)
                StartCoroutine(ToggleLight(lights[index]));
        }
    }
    public IEnumerator ToggleLight(GameObject light)
    {
        light.SetActive(false);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0,2));
        light.SetActive(true);
    }
}
