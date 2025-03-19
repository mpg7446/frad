using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptidUtils : MonoBehaviour
{
    protected const float pi = Mathf.PI; // just in case i need it lmao
    protected void LogWarning(string text) { Debug.LogWarning(this.GetType().ToString() + ": " + text); }
    protected void LogWarning(char text) { Log(text + ""); }
    protected void LogWarning(int text) { Log(text + ""); }
    protected void LogError(string text) { Debug.LogError(this.GetType().ToString() + ": " + text); }
    protected void LogError(char text) { Log(text + ""); }
    protected void LogError(int text) { Log(text + ""); }
    protected void LogError(Exception e) { Log(e + "");  }
    protected void Log(string text) { Debug.Log(this.GetType().ToString() + ": " + text);  }
    protected void Log(char text) { Log(text + ""); }
    protected void Log(int text) { Log(text + "");  }

    // I don't think I'll need these uwu
    protected Transform RelativeRotation(GameObject obj, Vector3 rot) 
    {
        Transform transform = obj.transform;
        transform.eulerAngles = rot;
        return transform;
    }
    protected Transform RelativeRotation(Vector3 rot) { return RelativeRotation(this.gameObject, rot); }
    protected Transform RelativeRotation(float x, float y, float z) { return RelativeRotation(new Vector3(x, y, z));  }
}
