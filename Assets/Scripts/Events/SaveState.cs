using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : ScriptableObject {
    public readonly Vector3 playerPosition;
    public readonly Quaternion playerRotation;
    public readonly List<Enemy.ID> activeEnemies;
    public readonly List<Vector3> enemyLocations;
    public readonly List<Vector3> enemyDestinations;
    private readonly bool[] eventHistory;

    public SaveState(Transform playerLocation, List<Enemy.ID> activeEnemies, List<Vector3> enemyLocations, List<Vector3> enemyDestinations, List<Event> triggeredEvents) {
        playerPosition = playerLocation.position;
        playerRotation = playerLocation.rotation;
        this.activeEnemies = activeEnemies;
        this.enemyLocations = enemyLocations;
        this.enemyDestinations = enemyDestinations;

        if (triggeredEvents != null && triggeredEvents.Count != 0) 
            for (int i = 0; i < triggeredEvents.Count; i++)
                eventHistory[i] = triggeredEvents[i].state == Event.PlayState.Played;
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
