using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Husk : SmartEnemy {
    [SerializeField] protected float searchSpeed;
    [SerializeField] protected float chaseSpeed;

    protected int searchCounter = 0;
    protected bool canSearch = true;
    [Tooltip("Amount of times the AI can search through the room")]
    [SerializeField] protected int maxSearchTimer = 3;
    protected bool isProcessing = false;
    protected int targetViewTimer = 0;
    [Tooltip("Time (in seconds) the AI has to be able to see the player before chasing")]
    [SerializeField] protected float targetViewMax = 1f;

    #region StepUpdate functions
    protected override void StepUpdate() {
        if (targetViewTimer > 0)
            targetViewTimer -= fixedStepRate/2;
    }
    protected override void StepNone() {
        Roam();
    }
    protected override void StepRoam() {
        if (ReachedDestination) {
            if (canSearch && !PlayerManager.Instance.InLocker && PlayerManager.Instance.room == Room) {
                Log("Setting state to Search (In Room)");
                Search();
                return;
            }
            ChangeTargetRoom();
        }
    }
    protected override void StepSearch() {
        if (!isProcessing && ReachedDestination) {
            searchCounter--;
            if (searchCounter <= 0) {
                Roam();
                canSearch = false;
                return;
            }
            StartCoroutine(ContinueSearch());
        }
    }
    protected IEnumerator ContinueSearch() {
        // pls call waitying animtiong here pls and thank u uwu <3
        agent.ResetPath();
        isProcessing = true;
        float delay = Random.Range(2f, 6f);
        //Log("Search delay of " + delay);
        yield return new WaitForSeconds(delay);

        agent.SetDestination(GetRandomRoomSpot());
        isProcessing = false;
    }

    protected IEnumerator StartProcessing(float min = 2f, float max = 6f) {
        isProcessing = true;
        agent.enabled = false;
        yield return new WaitForSeconds(Random.Range(min, max));
        agent.enabled = true;
        isProcessing = false;
    }

    protected override void StepChase() {
        if (!isProcessing) {
            if (InViewRadius) {
                agent.SetDestination(target.transform.position);
            } else if (ReachedDestination) {
                Roam();
            }
        }
    }

    protected override void StepDetected() {
        if (!PlayerManager.Instance.InLocker && canSearch && state == State.Roam) {
            Room = PlayerManager.Instance.room;
            Search();
        }
    }
    protected override void StepSeen() {
        Log("target is in view radius");
        if (state != State.Chase) {
            Log("updating targetViewTimer");
            targetViewTimer += fixedStepRate;
            if (targetViewTimer > (50 / fixedStepRate) * targetViewMax) {
                targetViewTimer = 0;
                StartCoroutine(StartProcessing());
                Chase();
            }
        }
    }
    #endregion

    #region State change triggers
    protected override void OnTargetRoomChange() {
        agent.SetDestination(GetRandomRoomSpot());
    }

    protected override void OnCurrentRoomChange() {
        if (!canSearch)
            canSearch = true;
    }

    protected override void OnRoam() {
        agent.speed = speed;
        ChangeTargetRoom();
    }

    protected override void OnSearch() {
        Log("Setting state to Search");
        agent.speed = searchSpeed;
        searchCounter = maxSearchTimer;
        StartCoroutine(ContinueSearch());
    }

    protected override void OnChase() {
        agent.speed = chaseSpeed;
    }

    protected override void OnCollide() {
        if (!PlayerManager.Instance.InLocker) {
            Log("Husk hit player");
            PlayerManager.Instance.Damage(damage, damagesPlayerHealth, damagesPlayerHelmet);
        }
    }

    protected override void OnNone() { }
    #endregion
}
