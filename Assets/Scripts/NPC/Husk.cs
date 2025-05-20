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

    #region StepUpdate functions
    protected override void OnNone() {
        Roam();
    }
    protected override void OnRoam() {
        if (ReachedDestination) {
            if (canSearch && !PlayerManager.Instance.InLocker && PlayerManager.Instance.room == Room) {
                Search();
                return;
            }
            ChangeTargetRoom();
        }
    }
    protected override void OnSearch() {
        if (ReachedDestination) {
            searchTimer--;
            if (searchTimer <= 0) {
                Roam();
                canSearch = false;
                return;
            }
            agent.SetDestination(GetRandomRoomSpot());
        }
    }
    protected override void OnChase() { }

    protected override void Detected() {
        if (state == State.Roam)
            Search();
    }
    protected override void Seen() {
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
        agent.speed = searchSpeed;
        agent.SetDestination(GetRandomRoomSpot());
        searchTimer = maxSearchTimer;
    }

    protected override void Chase() {
        base.Chase();
        agent.speed = chaseSpeed;
    }
    #endregion
}
