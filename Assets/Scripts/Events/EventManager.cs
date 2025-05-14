using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : CryptidUtils {
    public static EventManager Instance;
    public List<Event> events = new();

    public SaveState lastState;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");
    }

    public void RegisterEvent(Event e) => events.Add(e);

    [ContextMenu("Force Save")]
    public void Save() {
        // get enemy data
        List<Enemy.ID> active = new();
        List<Vector3> enemyLocations = new();
        List<Vector3> enemyDestinations = new();
        try {
            foreach (Enemy e in Director.Instance.Active) {
                active.Add(e.EnemyID);
                enemyLocations.Add(e.gameObject.transform.position);
                enemyDestinations.Add(e.agent.destination);
            }
        } catch {
            LogWarn("No active enemies found in scene");
        }

        // get event data
        List<Event> triggered = new();

        for (int i = 0; i < events.Count; i++) {
            if (events[i].state != Event.PlayState.Ready) {
                triggered.Add(events[i]);
                Log("Saved triggered event: " + events[i].name);
            }
        }

        //lastState = new(PlayerManager.Instance.transform, active, enemyLocations, enemyDestinations, triggered); // replace 'new List<Event>()' with a list of triggered events
        lastState = ScriptableObject.CreateInstance<SaveState>();
        lastState.Save(PlayerManager.Instance.transform, active, enemyLocations, enemyDestinations, triggered);
    }

    [ContextMenu("Rollback Last Save")]
    public void Rollback() {
        // this is the part where he kills u <3
        if (lastState == null) {
            LogWarn("Failed to rollback save, no save state found");
            return;
        }

        Log("lets pretend theres a function here");
        // reset player position
        PlayerManager.Instance.gameObject.transform.SetPositionAndRotation(lastState.PlayerPosition, lastState.PlayerRotation);
        PlayerManager.Instance.agent.ResetPath();

        // set enemy positions
        if (lastState.ActiveEnemies != null && lastState.ActiveEnemies.Count > 0) {
            for (int i = 0; i < lastState.ActiveEnemies.Count; i++) {
                Enemy enemy = Director.Instance.Active[i];
                enemy.transform.position = lastState.EnemyLocations[i];
                enemy.agent.SetDestination(lastState.EnemyDestinations[i]);
            }
        }

        // rollback triggered events
    }
}
