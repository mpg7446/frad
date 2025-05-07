using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Director : CryptidUtils {
    public static Director Instance;
    public List<Enemy> Active { get; private set; }

    [Header("NPC Objects")]
    [SerializeField] private GameObject swarm;
    [SerializeField] private GameObject husk;
    [SerializeField] private GameObject dens; // probably not gonna use this
    // OR this could be used as a punishment for not following objectives

    // easter egg / joke NPCs
    [SerializeField] private GameObject fazball;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");
    }

    private GameObject GetNPC(Enemy.ID id) {
        switch (id) {
            case Enemy.ID.Swarm:
                return swarm;
            case Enemy.ID.Husk:
                return husk;
            case Enemy.ID.Dens:
                return dens;
            case Enemy.ID.Fazball:
                return fazball;
            default:
                break;
        }
        return null;
    }

    public void Spawn(Enemy.ID id, Vector3 location) {
        GameObject instance = Instantiate(GetNPC(id));
        Active.Add(instance.GetComponent<Enemy>());
        instance.transform.position = location;
        try {
            instance.GetComponent<Enemy>().target = PlayerManager.Instance.gameObject;
        }
        catch {
            instance.GetComponent<FollowingNPC>().target = PlayerManager.Instance.gameObject;
        }
    }

    public void RegisterPing() { /*
        foreach (Enemy enemy in Active) {
            if (enemy is SmartEnemy) {
                Log("lets just pretend that something happens here");
                // this is where i watched my parents die parapa
                // check where player is compared to enemy and sight and shit checks and whatnot
            }
        }*/
    }
}