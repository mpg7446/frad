using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestingEnemy : SmartEnemy {
    private float radiusDelay;
    public float detectionTime;
    [SerializeField] private float sprintSpeed;
    //protected override void InitBrain() => brain = ScriptableObject.CreateInstance<Brain>();

    protected override void StepNone() => OnRoam();
    protected override void StepRoam() {
        Vector3 loc = target.transform.position;
        loc.x += Random.Range(-20, 20);
        loc.z += Random.Range(-20, 20);
        agent.SetDestination(loc);
        agent.speed = speed;
        OnSearch();
    }
    protected override void StepSearch() {
        if (ReachedDestination) OnRoam();
    }
    protected override void StepChase() {
        agent.SetDestination(target.transform.position);

        if (ReachedDestination) OnRoam();
    }
    protected override void StepDetected() {
        if (state != State.Chase) {
            if (radiusDelay < detectionTime*8){
                radiusDelay++;
                return;
            }
            radiusDelay--;
            OnChase();
        }
    }
    protected override void StepSeen() {
        if (state != State.Chase) OnChase();
    }

    protected override void OnRoam() {
        agent.speed = speed;
        //agent.stoppingDistance = 1;
    }
    protected override void OnChase() {
        agent.speed = sprintSpeed;
        //agent.stoppingDistance = 0;
    }

    protected override void OnTargetRoomChange() {
        throw new System.NotImplementedException();
    }

    protected override void OnCurrentRoomChange() {
        throw new System.NotImplementedException();
    }

    protected override void OnCollide() {
        throw new System.NotImplementedException();
    }

    protected override void StepUpdate() {
        throw new System.NotImplementedException();
    }

    protected override void OnNone() {
        throw new System.NotImplementedException();
    }

    protected override void OnSearch() {
        throw new System.NotImplementedException();
    }
}
