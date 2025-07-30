using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public Enemy.ID EnemyID;
    private void Start() => GameManager.Instance.Register(this);
    private void OnDestroy() => GameManager.Instance.Deregister(this);
}
