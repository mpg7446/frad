using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerManager : MonoBehaviour
{
    public GameObject[] lights;

    private void FixedUpdate()
    {
        int index = UnityEngine.Random.Range(0, lights.Length+1);
        try
        {
            if (lights[index].activeSelf)
                StartCoroutine(ToggleLight(lights[index]));
        } catch { }
    }
    public IEnumerator ToggleLight(GameObject light)
    {
        light.SetActive(false);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0,2));
        light.SetActive(true);
    }
}
