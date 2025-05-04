using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Director : CryptidUtils
{
    public static Director Instance;
    public List<Enemy> Active { get; private set; }

    [Header("NPC Objects")]
    [SerializeField] private GameObject swarm;
    [SerializeField] private GameObject husk;
    [SerializeField] private GameObject dens;

    // easter egg / joke NPCs
    [SerializeField] private GameObject fazball;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject, "Instance already exists");
    }
    private GameObject GetNPC(Enemy.ID id)
    {
        switch (id)
        {
            case Enemy.ID.Swarm:
                return swarm;
            case Enemy.ID.Husk:
                return husk;
            case Enemy.ID.Dens:
                return dens;
            case Enemy.ID.Fazball:
                return fazball;
        }
        return null;
    }

    public void Spawn(Enemy.ID id, Vector3 location)
    {
        GameObject instance = Instantiate(GetNPC(id));
        Active.Add(instance.GetComponent<Enemy>());
        instance.transform.position = location;
        try
        {
            instance.GetComponent<Enemy>().target = PlayerManager.Instance.gameObject;
        }
        catch
        {
            instance.GetComponent<FollowingNPC>().target = PlayerManager.Instance.gameObject;
        }
    }
}