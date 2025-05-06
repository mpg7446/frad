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

    public void Save() {
        List<Enemy.ID> active = new();
        List<Vector3> enemyLoc = new();
        try {
            foreach (Enemy e in Director.Instance.Active) {
                active.Add(e.EnemyID);
                enemyLoc.Add(e.gameObject.transform.position);
            }
        } catch {
            LogWarn("No active enemies found in scene");
        }

        lastState = new(PlayerManager.Instance.transform.position, active.ToArray(), enemyLoc.ToArray(), new List<Event>()); // replace 'new List<Event>()' with a list of triggered events
        lastState.ass();
    }

    public void Rollback() {
        // this is the part where he kills u <3
    }
}
