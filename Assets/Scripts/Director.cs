using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director Instance;

    [Header("NPC Objects")]
    [SerializeField] private GameObject swarm;
    [SerializeField] private GameObject husk;
    [SerializeField] private GameObject dens;

    // easter egg / joke NPCs
    [SerializeField] private GameObject fazball;

    public enum ID
    {
        Swarm,
        Husk,
        Dens,
        Fazball
    }

    private void Start()
    {
        Instance = this;
    }
    private GameObject GetNPC(ID id)
    {
        switch (id)
        {
            case ID.Swarm:
                return swarm;
            case ID.Husk:
                return husk;
            case ID.Dens:
                return dens;
            case ID.Fazball:
                return fazball;
        }
        return null;
    }

    public void Spawn(ID id, Vector3 location)
    {
        GameObject instance = Instantiate(GetNPC(id));
        instance.transform.position = location;
        try
        {
            instance.GetComponent<Enemy>().target = PlayerManager.Instance.gameObject;
        } catch
        {
            instance.GetComponent<FollowingNPC>().target = PlayerManager.Instance.gameObject;
        }
    }
}
