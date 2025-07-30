using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valuable : InteractableObject {
    public int value;

    private void Start() {
        GameManager.Instance.Register(this);
    }
    protected override void OnInteract(GameObject sender) {
        PlayerManager.Instance.score += value;
        Destroy(gameObject, "Item Colected");
    }
}
