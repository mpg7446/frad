using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Husk : SmartEnemy {
    [SerializeField] protected float searchSpeed;
    [SerializeField] protected float chaseSpeed;

    protected int searchTimer = 0;
    protected bool canSearch = true;
    [Tooltip("Amount of times the AI can search through the room")]
    [SerializeField] protected int maxSearchTimer = 3;
    protected bool isProcessing = false;

    #region StepUpdate functions
    protected override void OnNone() {
        Roam();
    }
    protected override void OnRoam() {
        if (ReachedDestination) {
            if (canSearch && !PlayerManager.Instance.InLocker && PlayerManager.Instance.room == Room) {
                Log("Setting state to Search (In Room)");
                Search();
                return;
            }
            ChangeTargetRoom();
        }
    }
    protected override void OnSearch() {
        if (!isProcessing && ReachedDestination) {
            searchTimer--;
            if (searchTimer <= 0) {
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
        Log("Search delay of " + delay);
        yield return new WaitForSeconds(delay);

        agent.SetDestination(GetRandomRoomSpot());
        isProcessing = false;
    }

    protected override void OnChase() { }

    protected override void Detected() {
        if (canSearch && state == State.Roam) {
            Room = PlayerManager.Instance.room;
            Search();
        }
    }
    protected override void Seen() {
        if (isProcessing && state != State.Chase) {
            Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
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

    protected override void Roam() {
        base.Roam();
        agent.speed = speed;
        ChangeTargetRoom();
    }

    protected override void Search() {
        base.Search();
        Log("Setting state to Search");
        agent.speed = searchSpeed;
        searchTimer = maxSearchTimer;
        StartCoroutine(ContinueSearch());
    }

    protected override void Chase() {
        base.Chase();
        agent.speed = chaseSpeed;
    }
    #endregion
}
