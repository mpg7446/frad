using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : ScriptableObject
{
    public enum PingType {
        Sound,
        Event,
        Locker
    }
    public float size = 1f;
    public PingType type = PingType.Sound;
    public Vector3 position;

    public static Ping Create(PingType type, Vector3 position, float size = 1f) {
        if (size < 0) {
            Debug.LogError("Ping.Set: Unable to set ping daya, float cannot be negative!");
            return null;
        }
        Ping ping = (Ping)ScriptableObject.CreateInstance(typeof(Ping));
        ping.size = size;
        ping.position = position;
        ping.type = type;
        return ping;
    }
}
