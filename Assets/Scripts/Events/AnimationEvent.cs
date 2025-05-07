using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : Event {
    [SerializeField] private Animation anim;

    public PlayType type = PlayType.OnEnter;
    public enum PlayType {
        OnEnter,
        OnExit,
        Contained
    }
    [Tooltip("Defines whether to disable the event after the animation has played.")]
    public bool persistant = false;
    public bool Triggered {
        get {
            return state != PlayState.Ready;
        }
    }

    protected override void OnStart() { }

    private void FixedUpdate() {
        if (state == PlayState.Playing && type != PlayType.Contained && !anim.isPlaying)
            Stop();
    }
    protected override void OnEnter() {
        if (state == PlayState.Ready && type != PlayType.OnExit)
            Play();
    }

    protected override void OnExit() {
        if (state == PlayState.Ready && type == PlayType.OnExit)
            Play();
        else if (state == PlayState.Ready && type == PlayType.Contained)
            Stop();
    }

    private void Play() {
        anim.Play();
        state = PlayState.Playing;
    }
    private void Stop() {
        anim.Stop();
        state = PlayState.Played;

        if (!persistant)
            gameObject.SetActive(false);
    }

}
