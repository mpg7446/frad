using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : Event {
    protected override void OnEnter() => GameManager.Instance.LoadNextMap();
    protected override void OnExit() { }
    protected override void OnStart() { }
}
