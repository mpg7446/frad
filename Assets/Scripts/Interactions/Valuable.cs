using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valuable : InteractableObject {
    public int value;

    private void Start() {
        GameManager.Instance.maxScore += value;
    }
    protected override void OnInteract(GameObject sender) {
        PlayerManager.Instance.score += value;
    }
}
