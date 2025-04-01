using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : ScriptableObject
{
    public string identifier = null;
    public LinkedList<Memory> memories;
    public float aggression;

    #region Loading
    public Brain(string identifier, float startingAggro = 0)
    {
        this.identifier = identifier;
        aggression = startingAggro;
    }

    public void LoadMemories(LinkedList<Memory> memories)
    {
        this.memories = memories;
    }
    public void LoadMemories(Memory[] memories)
    {
        this.memories = new LinkedList<Memory>();
        foreach (Memory memory in memories)
            this.memories.AddLast(memory);
    }
    #endregion
}
