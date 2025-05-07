using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Event {
    protected override void OnStart() { }
    protected override void OnEnter() => Save();
    protected override void OnExit() { }

    private void Save() {
        Log("reached checkpoint " + name);
        state = PlayState.Played;
        EventManager.Instance.Save();
        gameObject.SetActive(false);
    }
}
