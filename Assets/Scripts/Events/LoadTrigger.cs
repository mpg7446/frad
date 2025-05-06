using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : Event {
    public Scene scene;
    protected override void OnEvent() => GameManager.Instance.LoadNextMap();
    protected override void OnExitEvent() { }
    protected override void OnStart() { }
}
