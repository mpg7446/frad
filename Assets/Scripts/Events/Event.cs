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

    private void Awake() {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        col.includeLayers = collisionMask;
    }
    private void Start() {
        EventManager.Instance.RegisterEvent(this);
        OnStart();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            OnEvent();
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            OnExitEvent();
    }

    protected abstract void OnStart();
    protected abstract void OnEvent();
    protected abstract void OnExitEvent();
}