using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public Enemy.ID EnemyID;
    private void Start() => GameManager.Instance.RegisterEnemySpawner(this);
    private void OnDestroy() => GameManager.Instance.DeregisterEnemySpawner(this);
}
