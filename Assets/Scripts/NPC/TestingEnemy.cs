using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestingEnemy : Enemy
{
    private int radiusDelay;
    protected override void InitBrain()
    {
        brain = ScriptableObject.CreateInstance<Brain>();
    }

    protected override void OnNone()
    {
        Roam();
    }
    protected override void OnRoam()
    {
        Vector3 loc = target.transform.position;
        loc.x += Random.Range(-20, 20);
        loc.z += Random.Range(-20, 20);
        agent.SetDestination(loc);
        agent.speed = speed;
        Search();
    }
    protected override void OnSearch()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            Roam();
    }
    protected override void OnChase()
    {
        agent.SetDestination(target.transform.position);

        if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            Roam();
    }
    protected override void Detected()
    {
        if (state != State.Chase)
        {
            if (radiusDelay < 3){
                radiusDelay++;
                return;
            }
            radiusDelay--;
            Chase();
        }
    }

    protected override void EnteredFrustum()
    {
        //agent.speed = sprintSpeed;
        //Chase();
    }
    protected override void LeftFrustum()
    {
        agent.speed = speed;
        Roam();
    }

    protected override void Roam()
    {
        base.Roam();
        agent.speed = speed;
        agent.stoppingDistance = 1;
    }
    protected override void Chase()
    {
        base.Chase();
        agent.speed = sprintSpeed;
        agent.stoppingDistance = 0;
    }
}
