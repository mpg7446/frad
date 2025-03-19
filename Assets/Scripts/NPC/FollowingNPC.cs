using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingNPC : CryptidUtils
{
    public GameObject target;
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
        transform.position += transform.forward * speed * (Vector3.Distance(transform.position, target.transform.position)/5) / 100;
    }
}
