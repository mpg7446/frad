using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public GameObject target;
    //private Quaternion offset;

    //private void Start()
    //{
    //    offset = transform.rotation;
    //}
    void Update()
    {
        //transform.rotation = offset; // make this add the rotation too pls and thank u <3
        transform.LookAt(target.transform.position);
    }
}
