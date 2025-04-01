using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingEnemy : Enemy
{
    protected override void initBrain()
    {
        brain = new Brain("Testing Enemy");
    }

    protected override void OnNone()
    {
        state = State.Roam;
    }
    protected override void OnRoam()
    {
        throw new System.NotImplementedException();
    }
    protected override void OnSearch()
    {
        throw new System.NotImplementedException();
    }
    protected override void OnChase()
    {
        throw new System.NotImplementedException();
    }
}
