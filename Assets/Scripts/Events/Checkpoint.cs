using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Event {
    protected override void OnStart() { }
    protected override void OnEvent() => Save();
    protected override void OnExitEvent() { }

    private void Save() {
        Log("reached checkpoint " + name);
        state = PlayState.Played;
        EventManager.Instance.Save();
        gameObject.SetActive(false);
    }
}
