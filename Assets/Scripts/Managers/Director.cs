using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : CryptidUtils {
    public static Director Instance;
    public int intensity = 0;

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

        Active = new();
    }

    public void Pause() {

    }

    public void Play() {

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

    public void Spawn(EnemySpawner spawner) {
        GameObject instance = Instantiate(GetNPC(spawner.EnemyID));
        Active.Add(instance.GetComponent<Enemy>());
        instance.GetComponent<Enemy>().target = PlayerManager.Instance.gameObject;

        instance.transform.SetPositionAndRotation(spawner.setPosition ? spawner.transform.position : instance.transform.position, 
            spawner.setRotation ? spawner.transform.rotation : instance.transform.rotation);
        SceneManager.MoveGameObjectToScene(instance, spawner.gameObject.scene);
    }

    // i spent way too long trying to figure out what i was trying to do here by the comments
    // i swear i was high or something the fuck is this supposed to mean???
    public void RegisterPing(Ping ping) {
        if (Active.Count <= 0)
            return;

        if (ping.type == Ping.PingType.Locker) {
            foreach (Enemy enemy in Active) {
                if (enemy is SmartEnemy e) {
                    e.RegisterLockerPing(ping.position);
                }
            }
        }

        /*foreach (Enemy enemy in Active) {
            if (enemy is SmartEnemy) {
                Log("lets just pretend that something happens here");
                // this is where i watched my parents die parapa
                // check where player is compared to enemy and sight and shit checks and whatnot
            }
        }*/
    }
}