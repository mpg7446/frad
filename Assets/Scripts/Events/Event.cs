using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Event : CryptidUtils {
    public LayerMask collisionMask;
    public PlayState state = PlayState.Ready;
    public enum PlayState {
        Ready,
        Playing,
        Played
    }

    private void Start() {
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger) {
            LogWarning("Collider on " + name + " is not set as trigger, setting to trigger");
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == collisionMask) {
            OnEventTrigger();
            Log("Event Triggered");
        }
    }
    public abstract void OnEventTrigger();
}