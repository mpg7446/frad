using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class SmartEnemy : Enemy {
    public Room Room;
    protected List<Room> roomBag = new();
    protected int _roomBagIndex = 0;

    protected bool watchesLockers;
    protected int lockerValue;

    protected float paranoia; // paranoia increases with close encounters with the player and makes movements more sparattic
    // should release over time when far enough from player

    // TODO
    // PLEASE OMG PLEASE add a feature where the AI "dukes" the player by starting to head in one direction and then snapping back to catch the player off gaurd
    // make this happen after a certain amount of time and/or amount of times the player has escaped the AI

    protected override void Start() {
        base.Start();
        gameObject.tag = "Smart Enemy";
    }

    public void ChangeCurrentRoom(Room room) {
        //Log($"Entered new room \"{room.name}\"");
        Room = room;
        OnCurrentRoomChange();
    }

    public void ChangeTargetRoom() {
        _roomBagIndex++;
        if (_roomBagIndex >= roomBag.Count) {
            GenerateRoomBag();
            return;
        }

        OnTargetRoomChange();
    }

    protected void GenerateRoomBag() {
        _roomBagIndex = 0;
        roomBag = new();
        foreach (Room room in GameManager.Instance.rooms)
            roomBag.Add(room);

        // add randomized rooms for making AI run back to check -- OBSOLETE SO FAR
        /*int index = UnityEngine.Random.Range(1, roomBag.Count);
        int[] roomID = new int[index];
        for (int i = 0; i < index; i++) {
            roomID[i] = UnityEngine.Random.Range(0, roomBag.Count - 1);
            roomBag.InsertRange(index, (IEnumerable<Room>)GameManager.Instance.rooms[roomID[i]]);
        }*/
    }

    protected Vector3 GetRandomRoomSpot(Room room) {
        Vector3 min = room.Collider.bounds.min;
        Vector3 max = room.Collider.bounds.max;

        return new(UnityEngine.Random.Range(min.x,max.x), 
            UnityEngine.Random.Range(min.y, max.y), 
            UnityEngine.Random.Range(min.z, max.z));
    }
    protected Vector3 GetRandomRoomSpot() => GetRandomRoomSpot(roomBag[_roomBagIndex]);

    public void RegisterLockerPing(Vector3 position) {
        if (watchesLockers && (Vector3.Distance(position, transform.position) < detectionRadius * 1.2 || PlayerManager.Instance.room == Room)) {
            lockerValue++;
        }
    }

    #region override functions
    protected abstract void OnTargetRoomChange();
    protected abstract void OnCurrentRoomChange();
    #endregion
}
