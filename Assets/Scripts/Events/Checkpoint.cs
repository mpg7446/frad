using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Event
{
    public override void OnEventTrigger() => Save();
    private void Save()
    {
        Log("reached checkpoint " + name);
        state = PlayState.Played;
        EventManager.Instance.Save();
        gameObject.SetActive(false);
    }
}
