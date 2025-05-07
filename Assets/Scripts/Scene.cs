using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene Script", fileName = "New Scene")]
public class Scene : ScriptableObject
{
    public string sceneName;
    public SceneType type;
    public string exclusiveTag;
    public bool isExclusive { get {
            return type == SceneType.Exclusive;
        } 
    }

    public enum SceneType {
        Normal,
        Exclusive,
        Persistent
    }
}
