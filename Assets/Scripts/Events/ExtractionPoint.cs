using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionPoint : Event
{
    protected override void OnEnter() => GameManager.Instance.canExtract = true;

    protected override void OnExit() => GameManager.Instance.canExtract = false;

    protected override void OnStart() {}
}
