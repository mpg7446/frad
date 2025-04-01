using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CryptidUtils
{
    protected Brain brain;
    [SerializeField] protected Collider viewFrustum;
    [SerializeField] protected Collider areaSenseCollider;
    [SerializeField] protected State state = State.None;
    protected enum State // this will define what the AI is doing
    {
        None,
        Roam,
        Search,
        Chase
    }

    private void Start()
    {
        initBrain();

        // Check if colliders are set correctly
        if (viewFrustum == null || areaSenseCollider == null) {
            LogError("Missing colliders for detecting player!");
            Destroy(gameObject);
        } else if (!viewFrustum.isTrigger || !areaSenseCollider.isTrigger)
        {
            LogError("One or more colliders for detecting player is not set to trigger!");
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.None:
                OnNone();
                break;
            case State.Roam:
                OnRoam();
                break;
            case State.Search:
                OnSearch();
                break;
            case State.Chase:
                OnChase();
                break;
        }
    }

    #region Override Methods
    // Brain object used for storing previous player - will be parsed through enemy manager or save state manager on destroy
    // Override this method to set the enemy identifier
    // brain = new Brain(identifier);
    protected abstract void initBrain();

    // These methods are called every FixedUpdate
    // which one is called depends on the State state
    protected abstract void OnNone();
    protected abstract void OnRoam();
    protected abstract void OnSearch();
    protected abstract void OnChase();
    #endregion
}
