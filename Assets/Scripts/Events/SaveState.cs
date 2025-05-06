using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : ScriptableObject {
    public readonly Vector3 playerLoc;
    public readonly Enemy.ID[] activeEnemies;
    public readonly Vector3[] enemyLoc;
    private readonly bool[] eventHistory;

    public SaveState(Vector3 playerLoc, Enemy.ID[] activeEnemies, Vector3[] enemyLoc, List<Event> triggeredEvents) {
        this.playerLoc = playerLoc;
        this.activeEnemies = activeEnemies;
        this.enemyLoc = enemyLoc;

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

    public void ass() {
        Debug.Log("this code is ass, i cant figure out how im gonna return the save state qwq");
    }
}
