using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
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

    [Space]
    [Header("NPC Settings")]
    [Range(0, 100)]
    public float damage;
    public bool damagesPlayerHealth;
    public bool damagesPlayerHelmet;
    [Tooltip("How many FixedUpdate frames to skip between each AI movement update")]
    public int fixedStepRate = 8;
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

    [SerializeField] protected GameObject viewObject;
    [Tooltip("Field of view for line of sight detection")]
    [SerializeField] protected float FOV = 180;
    [Tooltip("Distance the AI can see for line of sight detection")]
    [SerializeField] protected float viewDistance = Mathf.Infinity;
    [Tooltip("LayerMask dictating what layers the line of sight checks")]
    [SerializeField] protected LayerMask raycastMask;
    protected bool InViewRadius { get {
            Vector3 heading = RelPos(viewObject.transform.position, targetCol.bounds.center);
            if (Vector3.Angle(heading, viewObject.transform.forward) <= FOV / 2) {
                Debug.DrawRay(viewObject.transform.position, Vector3.ClampMagnitude(heading,viewDistance));
                if (Physics.Raycast(viewObject.transform.position, heading, out RaycastHit hit, viewDistance, raycastMask))
                    return hit.collider.CompareTag("Player");
            }
            return false;
        }
    }

    protected virtual void Start() {
        // Nav Mesh Agent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Target Colliders
        targetCol = target.GetComponent<Collider>();
    }

    protected virtual void FixedUpdate() {
        if (fixedStep == 0) {
            StepUpdate();
            switch (state) {
                case State.None:
                    StepNone();
                    break;
                case State.Roam:
                    StepRoam();
                    break;
                case State.Search:
                    StepSearch();
                    break;
                case State.Chase:
                    StepChase();
                    break;
            }

            if (InDetectionRadius) {
                StepDetected();
            }
            if (InViewRadius) {
                StepSeen();
            }

            fixedStep = 8;
        } else
            fixedStep--;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            OnCollide();
    }

    private void MissingComponent(string text) {
        LogErr(text);
        Destroy(gameObject);
    }

    #region State Control Methods
    protected void None() {
        state = State.None;
        OnNone();
    }
    protected void Roam() {
        state = State.Roam;
        OnRoam();
    }
    protected void Search() {
        state = State.Search;
        OnSearch();
    }
    protected void Chase() {
        state = State.Chase;
        OnChase();
    }
    #endregion

    #region Override Methods
    // Step methods are called every StepUpdate
    // which one is called depends on the State state except for StepUpdate
    // StepUpdate is called before the state update functions
    protected abstract void StepUpdate();
    protected abstract void StepNone();
    protected abstract void StepRoam();
    protected abstract void StepSearch();
    protected abstract void StepChase();
    protected abstract void StepDetected();
    protected abstract void StepSeen();

    // On methods are called on changes to state / collision
    protected abstract void OnNone();
    protected abstract void OnRoam();
    protected abstract void OnSearch();
    protected abstract void OnChase();
    protected abstract void OnCollide();
    #endregion
}
