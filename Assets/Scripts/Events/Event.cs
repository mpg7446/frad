using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Event : CryptidUtils {
    public LayerMask collisionMask;
    public string collisionTag = "Player";
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
        if (other.gameObject.CompareTag(collisionTag))
            OnEnter();
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag(collisionTag))
            OnExit();
    }

    protected abstract void OnStart();
    protected abstract void OnEnter();
    protected abstract void OnExit();
}