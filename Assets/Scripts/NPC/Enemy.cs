using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : CryptidUtils {
    public ID EnemyID;
    public enum ID
    {
        Swarm,
        Husk,
        Dens,
        Fazball
    }

    [Header("Agent Settings")]
    [Tooltip("Dictates the AI Movement state")]
    [SerializeField] protected State state = State.None;
    public NavMeshAgent agent;
    [Tooltip("The GameObject target that the AI tries to attack")]
    public GameObject target;
    protected Collider targetCol;
    [Tooltip("Base movement speed")]
    [SerializeField] protected float speed;
    [Tooltip("How short of a distance between the AI and the destination for it to stop tracking")]
    [SerializeField] protected float destinationDistance = 0.4f;
    private int fixedStep;
    protected bool ReachedDestination { get { return (Vector3.Distance(transform.position, agent.destination) <= destinationDistance) || agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid; } }
    protected enum State // this will define what the AI is doing
    {
        None,
        Roam,
        Search,
        Chase
    }

    [Space]
    [Header("Player Detection")]
    [Tooltip("Distance the AI can detect the target without line of sight")]
    [SerializeField] protected float detectionRadius = 2;
    protected bool InDetectionRadius { get { return Vector3.Distance(transform.position, target.transform.position) <= detectionRadius; } }
    [Tooltip("Field of view for line of sight detection")]
    [SerializeField] protected float FOV = 180;
    [Tooltip("Distance the AI can see for line of sight detection")]
    [SerializeField] protected float viewDistance = Mathf.Infinity;
    [Tooltip("LayerMask dictating what layers the line of sight checks")]
    [SerializeField] protected LayerMask raycastMask;
    protected bool InViewRadius { get {
            Vector3 heading = RelPos(transform.position, targetCol.bounds.center);
            if (Vector3.Angle(heading, transform.right) <= FOV / 2) {
                Debug.DrawRay(transform.position, Vector3.ClampMagnitude(heading,viewDistance));
                if (Physics.Raycast(transform.position, heading, out RaycastHit hit, viewDistance, raycastMask))
                    return hit.collider.CompareTag("Player");
            }
            return false;
        }
    }

    protected virtual void Start() {
        // Nav Mesh Agent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            MissingComponent("Missing Nav Mesh Agent!");

        // Target Colliders
        targetCol = target.GetComponent<Collider>();
        if (targetCol == null)
            MissingComponent("Target is missing collider!");
    }

    protected virtual void FixedUpdate() {
        if (fixedStep == 0) {
            switch (state) {
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

            if (InDetectionRadius) {
                Detected();
            }
            if (InViewRadius) {
                Seen();
            }

            fixedStep = 8;
        } else
            fixedStep--;
    }

    private void MissingComponent(string text) {
        LogErr(text);
        Destroy(gameObject);
    }

    protected virtual void None() { state = State.None; }
    protected virtual void Roam() { state = State.Roam; }
    protected virtual void Search() { state = State.Search; }
    protected virtual void Chase() {  state = State.Chase; }

    #region Override Methods
    // These methods are called every FixedUpdate
    // which one is called depends on the State state
    protected abstract void OnNone();
    protected abstract void OnRoam();
    protected abstract void OnSearch();
    protected abstract void OnChase();

    // Entered methods are called OnTriggerEnter depending on object
    protected abstract void Detected();
    protected abstract void Seen();
    #endregion
}
