using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : CryptidUtils
{
    protected Brain brain;
    //[SerializeField] protected Collider viewFrustum;
    //[SerializeField] protected Collider areaSenseCollider;
    protected int areaTime;
    [SerializeField] protected State state = State.None;
    protected NavMeshAgent agent;
    [SerializeField] protected GameObject target;
    [SerializeField] protected float speed;
    [SerializeField] protected float sprintSpeed;
    private int fixedStep;
    [SerializeField] protected float detectionRadius = 2;
    protected bool InDetectionRadius {  get; private set; }
    protected enum State // this will define what the AI is doing
    {
        None,
        Roam,
        Search,
        Chase
    }

    private void Start()
    {
        InitBrain();

        // Nav Mesh Agent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            MissingComponent("Missing Nav Mesh Agent!");

        // Check if colliders are set correctly
        //if (viewFrustum == null || areaSenseCollider == null)
        //    MissingComponent("Missing colliders for detecting player!");
        //else if (!viewFrustum.isTrigger || !areaSenseCollider.isTrigger)
        //    MissingComponent("One or more colliders for detecting player is not set to trigger!");
    }

    private void FixedUpdate()
    {
        if (fixedStep == 0)
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

            if (InDetectionRadius)
            {
                Detected();
            }

            fixedStep = 25;
        } else
            fixedStep--;

        InDetectionRadius = Vector3.Distance(transform.position, target.transform.position) <= detectionRadius;
    }

    private void MissingComponent(string text)
    {
        LogError(text);
        Destroy(gameObject);
    }

    protected virtual void Roam() { state = State.Roam; }
    protected virtual void Search() { state = State.Search; }
    protected virtual void Chase() {  state = State.Chase; }

    #region Override Methods
    // Brain object used for storing previous player - will be parsed through enemy manager or save state manager on destroy
    // Override this method to set the enemy identifier
    // brain = new Brain(identifier);
    protected abstract void InitBrain();

    // These methods are called every FixedUpdate
    // which one is called depends on the State state
    protected abstract void OnNone();
    protected abstract void OnRoam();
    protected abstract void OnSearch();
    protected abstract void OnChase();

    // Entered methods are called OnTriggerEnter depending on object
    protected abstract void EnteredFrustum();
    protected abstract void LeftFrustum();
    protected abstract void Detected();
    #endregion
}
