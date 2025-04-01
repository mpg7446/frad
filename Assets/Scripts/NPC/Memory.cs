using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : ScriptableObject
{
    public float weight;
    public string descriptor = "I have no idea what this is really for just yet lmao";

    public Memory(float weight, string descriptor)
    {
        this.weight = weight;
        this.descriptor = descriptor;
    }
}
