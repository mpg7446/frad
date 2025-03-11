using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptidUtils : MonoBehaviour
{
    protected const float pi = Mathf.PI; // just in case i need it lmao
    protected void LogWarning(string warnText) { Debug.LogWarning(this.GetType().ToString() + ": " + warnText); }
    protected void LogError(string errorText) { Debug.LogError(this.GetType().ToString() + ": " + errorText); }
    protected void Log(string logText) { Debug.Log(this.GetType().ToString() + ": " + logText);  }

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
