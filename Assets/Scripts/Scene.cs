using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene Script", fileName = "New Scene")]
public class Scene : ScriptableObject
{
    public string sceneName;
    public sceneType type;
    public string exclusiveTag;
    public bool isExclusive { get {
            return type == sceneType.Exclusive;
        } 
    }

    public enum sceneType {
        Normal,
        Exclusive,
        Persistent
    }
}
