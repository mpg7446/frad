using System.Collections;
using System.Collections.Generic;
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

    public enum NPCID
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

    public void Spawn(NPCID NPC, Vector3 location)
    {

    }
}
