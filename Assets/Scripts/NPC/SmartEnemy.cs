using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartEnemy : Enemy
{
    protected Brain brain;

    protected override void Start()
    {
        InitBrain();
        base.Start();
    }

    #region override functions
    // Brain object used for storing previous player - will be parsed through enemy manager or save state manager on destroy
    // Override this method to set the enemy identifier
    // brain = new Brain(identifier);
    protected abstract void InitBrain();
    #endregion
}
