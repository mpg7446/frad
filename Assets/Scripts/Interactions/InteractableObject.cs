using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : CryptidUtils
{
    private void OnMouseEnter() {
        PlayerManager.Instance.lookingAt = this;
    }
    private void OnMouseExit() {
        if (PlayerManager.Instance.lookingAt == this)
            PlayerManager.Instance.lookingAt = null;
    }
}
