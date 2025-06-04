using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoad : MonoBehaviour
{
    void Start() {
        if (!gameObject.CompareTag("EditorOnly"))
            Debug.LogError($"DestroyOnLoad: Game Object ({name}) Tag not set as \"EditorOnly\", this could cause excess load times after building");
        Destroy(gameObject);
    }
}
