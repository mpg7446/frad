using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CryptidUtils : MonoBehaviour
{
    protected const float pi = Mathf.PI; // just in case i need it lmao
    protected Vector3 RelPos(Vector3 origin, Vector3 target) => target - origin;

    #region DebugLogs
    protected void LogWarn(string text) => Debug.LogWarning($"{GetType()} ({gameObject.name}): {text}");
    protected void LogWarn(char text) => Log(text + "");
    protected void LogWarn(int text) => Log(text + "");
    protected void LogWarn(Exception e) => Log(e + "");
    protected void LogErr(string text) => Debug.LogError($"{GetType()} ({gameObject.name}): {text}");
    protected void LogErr(char text) => Log(text + "");
    protected void LogErr(int text) => Log(text + "");
    protected void LogErr(Exception e) => Log(e + "");
    protected void Log(string text) => Debug.Log($"{GetType()} ({gameObject.name}): {text}"); 
    protected void Log(char text) => Log(text + "");
    protected void Log(int text) => Log(text + "");
    #endregion

    #region Object Management
    protected void Destroy(UnityEngine.Object obj, string text) => Destroy(obj, 0, text);
    protected void Destroy(UnityEngine.Object obj, int t, string text)
    {
        LogWarn("(" + GetType().ToString() + ") " + obj.name + " was destroyed with reason: \"" + text + "\"");
        Destroy(obj, t);
    }
    #endregion

    #region I don't think I'll need these uwu
    protected Transform RelativeRotation(GameObject obj, Vector3 rot) 
    {
        Transform transform = obj.transform;
        transform.eulerAngles = rot;
        return transform;
    }
    protected Transform RelativeRotation(Vector3 rot) { return RelativeRotation(this.gameObject, rot); }
    protected Transform RelativeRotation(float x, float y, float z) { return RelativeRotation(new Vector3(x, y, z));  }

    #endregion
}
