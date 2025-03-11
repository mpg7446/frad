using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public GameObject target;
    public bool rotation;
    public bool position;

    private float rotTime;
    void Update()
    {
        if (rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotTime);
            rotTime += Time.deltaTime;
        }

        if (position)
        {
            transform.position = target.transform.position;
        }
    }
}
