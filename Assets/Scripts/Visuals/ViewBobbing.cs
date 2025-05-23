using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobbing : CryptidUtils {
    [SerializeField] private float intensity = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float delay = 0;
    [SerializeField] private bool invert = false;
    private Vector3 origin;
    private Vector3 next;
    private float Amount { get {
            float a = (Mathf.Cos((PlayerManager.Instance.Step / 100 - (delay * 2)) * speed) * intensity) + intensity;
            return invert ? -a : a;
        } 
    }

    private void Start() {
        origin = transform.localPosition;
    }

    private void Update() {
        transform.localPosition = Vector3.Slerp(transform.localPosition, origin + next, Time.deltaTime);
    }

    private void FixedUpdate() {
        next = new Vector3(0, Amount, 0);
    }
}
