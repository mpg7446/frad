using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : CryptidUtils
{
    public float maxDistance = 1f;
    public float pingSize = 1f;
    private void OnMouseEnter() {
        PlayerManager.Instance.lookingAt = this;
    }
    private void OnMouseExit() {
        if (PlayerManager.Instance.lookingAt == this)
            PlayerManager.Instance.lookingAt = null;
    }
    public bool Interact(GameObject sender) {
        if (Vector3.Distance(sender.transform.position, transform.position) < maxDistance) {
            if (pingSize > 0)
                Director.Instance.RegisterPing(pingSize);
            OnInteract(sender);
            return true;
        }
        return false;
    }
    protected abstract void OnInteract(GameObject sender);
}
