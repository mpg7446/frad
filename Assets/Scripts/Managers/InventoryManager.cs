using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : CryptidUtils
{
    public static InventoryManager Instance;

    // item storage
    public ScriptableItem[] Inventory = new ScriptableItem[3];

    // available items
    [Space]
    [Header("Item Rolling")]
    [Tooltip("List of items available in end of game rolls")]
    [SerializeField] private List<ScriptableItem> Items = new();
    private readonly Dictionary<string, ScriptableItem> _items = new Dictionary<string, ScriptableItem>();

    // item selection/rolling
    private readonly ScriptableItem[] rollSlots = new ScriptableItem[3];
    private readonly ItemRollSlot[] rollSlotDisplays = new ItemRollSlot[3];

    public string[] GetInventory() {
        string[] data = new string[Inventory.Length];

        for (int i = 0; i < data.Length; i++) {
            if (Inventory[i] != null)
                data[i] = Inventory[i].internalName;
            else
                data[i] = null;
        }

        return data;
    }
    public void SetInventory(string[] data) {
        Inventory = new ScriptableItem[data.Length];

        for (int i = 0; i < data.Length; i++) {
            // no item found
            if (!string.IsNullOrEmpty(data[i]))
                Inventory[i] = _items[data[i]];
        }
    }

    private void Awake() {
        Instance = this;

        foreach (ScriptableItem item in Items) {
            _items.Add(item.internalName, item);
        }
    }

    public void RegisterRollSlot(ItemRollSlot rollSlot) {
        if (rollSlot.id >= rollSlotDisplays.Length || rollSlot.id < 0) {
            LogErr("Attempting to register ItemRollSlot, index out of bounds exception");
            return;
        }

        rollSlotDisplays[rollSlot.id] = rollSlot;
    }

    public void Clear() => Inventory = new ScriptableItem[3];

    public void Roll() { // add exception for action items?? - probably not
        for (int i = 0; i < rollSlots.Length; i++) {
            int id = Random.Range(0, Items.Count);
            ScriptableItem item = Items[id];

            rollSlots[i] = item;
            rollSlotDisplays[i].SetData(item.name, item.description);
        }
    }

    public void SelectItem(int slot) {
        if (slot >= Inventory.Length || slot < 0)
            return;

        Log($"Replacing Inventory Item in Slot #{slot + 1} ({slot}:{rollSlots[slot].name})");
        Inventory[slot] = rollSlots[slot];
    }
}
