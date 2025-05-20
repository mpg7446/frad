using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room : CryptidUtils
{
    public Collider Collider { get; private set; }
    public int ID { get; private set; }
    public static int _roomID;
    [SerializeField] public int importance; // TODO: this is to dictate which room overrides the other, putting an order of rooms in case of overlapping rooms

    private void Start() {
        ID = _roomID;
        _roomID++;

        Collider = GetComponent<Collider>();
        Collider.isTrigger = true;
        gameObject.isStatic = true;
        transform.name += $" (room.{ID})";

        GameManager.Instance.RegisterRoom(this);
    }
    private void OnDestroy() => GameManager.Instance.DeregisterRoom(this);

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            PlayerManager.Instance.room = this;
        else if (other.CompareTag("Smart Enemy"))
            other.GetComponent<SmartEnemy>().ChangeCurrentRoom(this);
        //switch (other.tag) {
        //    case "Player":
        //        PlayerManager.Instance.room = this;
        //        break;
        //    case "Smart Enemy":
        //        try {
        //            other.GetComponent<SmartEnemy>().ChangeCurrentRoom(this);
        //        } catch {
        //            LogWarn($"Unable to access SmartEnemy component on object \"{other.gameObject.name}\"");
        //        }
        //        break;
        //}
    }
}
