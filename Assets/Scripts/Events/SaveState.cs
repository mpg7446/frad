using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : ScriptableObject {
    public Vector3 PlayerPosition { get; private set; }
    public Quaternion PlayerRotation { get; private set; }
    public List<Enemy.ID> ActiveEnemies { get; private set; }
    public List<Vector3> EnemyLocations { get; private set; }
    public List<Vector3> EnemyDestinations { get; private set; }
    public readonly bool[] eventHistory;

    public void Save(Transform playerLocation, List<Enemy.ID> activeEnemies, List<Vector3> enemyLocations, List<Vector3> enemyDestinations, List<Event> triggeredEvents) {
        PlayerPosition = playerLocation.position;
        PlayerRotation = playerLocation.rotation;
        this.ActiveEnemies = activeEnemies;
        this.EnemyLocations = enemyLocations;
        this.EnemyDestinations = enemyDestinations;

        if (triggeredEvents != null && triggeredEvents.Count > 0) {
            for (int i = 0; i < triggeredEvents.Count; i++) {
                try {
                    eventHistory[i] = triggeredEvents[i].state == Event.PlayState.Played;
                } catch (Exception e) {
                    Debug.LogError($"I really dont know whats happening here, it should be saving the triggered event? but it calls an error???\nException: {e}");
                }
            }
        }
    }

    public List<Event> NewEvents(Event[] events) {
        List<Event> newEvents = null;
        int index = 0;

        foreach (Event e in events) {
            if ((e.state == Event.PlayState.Played && eventHistory[index]) || (e.state != Event.PlayState.Ready && !eventHistory[index]))
                newEvents.Add(e);
            index++;
        }

        return newEvents;
    }
}
