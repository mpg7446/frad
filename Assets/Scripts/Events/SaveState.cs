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
    public bool[] eventHistory;

    public void Save(Transform playerLocation, List<Enemy.ID> activeEnemies, List<Vector3> enemyLocations, List<Vector3> enemyDestinations, List<Event> triggeredEvents) {
        PlayerPosition = playerLocation.position;
        PlayerRotation = playerLocation.rotation;
        ActiveEnemies = activeEnemies;
        EnemyLocations = enemyLocations;
        EnemyDestinations = enemyDestinations;

        eventHistory = new bool[triggeredEvents.Count];
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

        foreach (Event @event in events) {
            if ((@event.state == Event.PlayState.Played && eventHistory[index]) || (@event.state != Event.PlayState.Ready && !eventHistory[index]))
                newEvents.Add(@event);
            index++;
        }

        return newEvents;
    }
}
