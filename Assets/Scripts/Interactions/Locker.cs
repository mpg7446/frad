using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : InteractableObject
{
    public GameObject exitPosition;
    protected override void OnInteract(GameObject sender) {
        Collider.enabled = false;
    }

    public void ExitLocker() {
        Collider.enabled = true;
    }
}
