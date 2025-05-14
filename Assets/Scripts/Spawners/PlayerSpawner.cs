using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    private void Start() => GameManager.Instance.RegisterPlayerSpawner(this);
    private void OnDestroy() => GameManager.Instance.DeregisterPlayerSpawner(this);
}
