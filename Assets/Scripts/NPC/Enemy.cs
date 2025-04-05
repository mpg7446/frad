using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : CryptidUtils
{
    protected Brain brain;

    [Header("Agent Settings")]
    [SerializeField] protected State state = State.None;
    protected NavMeshAgent agent;
    [SerializeField] protected GameObject target;
    protected Collider targetCol;
    [SerializeField] protected float speed;
    [SerializeField] protected float sprintSpeed;
    private int fixedStep;
    protected bool ReachedDestination { get { return (Vector3.Distance(transform.position, agent.destination) <= destinationDistance) || agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid; } }
    protected enum State // this will define what the AI is doing
    {
        None,
        Roam,
        Search,
        Chase
    }

    [Space(10)]
    [Header("Player Detection")]
    [SerializeField] protected float detectionRadius = 2;
    protected bool InDetectionRadius { get { return Vector3.Distance(transform.position, target.transform.position) <= detectionRadius; } }
    [SerializeField] protected float FOV = 180;
    [SerializeField] protected float viewDistance = Mathf.Infinity;
    [SerializeField] protected float destinationDistance = 0.4f;
    [SerializeField] protected LayerMask raycastMask;
    protected bool InViewRadius { get
        {
            Vector3 heading = targetCol.bounds.center - transform.position;
            if (Vector3.Angle(heading, transform.right) <= FOV / 2)
            {
                Debug.DrawRay(transform.position, heading);
                if (Physics.Raycast(transform.position, heading, out RaycastHit hit, viewDistance, raycastMask))
                    return hit.collider.CompareTag("Player");
            }
            return false;
        }
    }

    private void Start()
    {
        InitBrain();

        // Nav Mesh Agent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            MissingComponent("Missing Nav Mesh Agent!");

        // Target Colliders
        targetCol = target.GetComponent<Collider>();
        if (targetCol == null)
            MissingComponent("Target is missing collider!");
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
            if (InViewRadius)
            {
                Seen();
            }

            fixedStep = 8;
        } else
            fixedStep--;
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
    protected abstract void Seen();
    #endregion
}
