using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // item storage
    public ScriptableItem[] Inventory = new ScriptableItem[3];

    // item rolling
    [SerializeField] private List<ScriptableItem> Items = new();
    private readonly ScriptableItem[] rollSlots = new ScriptableItem[3];

    private void Start() {
        Instance = this;
    }

    public void Clear() => Inventory = new ScriptableItem[3];

    public void Roll() { // add exception for action items
        for (int i = 0; i < rollSlots.Length; i++) {
            int id = Random.Range(0, Items.Count);
            rollSlots[i] = Items[id];
        }
    }

    private void SelectItem(int slot) {
        if (slot > 3 || slot < 1)
            return;


    }

    public void SelectOne() => SelectItem(1);
    public void SelectTwo() => SelectItem(2);
    public void SelectThree() => SelectItem(3);
}
