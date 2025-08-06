using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : CryptidUtils
{
    public static InventoryManager Instance;

    // item storage
    // if count of items should be changed, please edit SaveData (Save.cs) to fit accordingly
    // or automate process
    public ScriptableItem[] Inventory = new ScriptableItem[3];

    // available items
    [Space]
    [Header("!!DO NOT REORDER!!")]
    [Tooltip("List of items, its order dictates save state UUID")]
    [SerializeField] private List<ScriptableItem> Items = new();

    // item selection/rolling
    private readonly ScriptableItem[] rollSlots = new ScriptableItem[3];
    private readonly ItemRollSlot[] rollSlotDisplays = new ItemRollSlot[3];

    public int[] GetInventory() {
        int[] data = new int[Inventory.Length];

        for (int i = 0; i < data.Length; i++) {
            if (Inventory[i] != null)
                data[i] = Inventory[i].ID;
            else
                data[i] = -1;
        }

        return data;
    }
    public void SetInventory(int[] data) {
        for (int i = 0; i < data.Length; i++) {
            if (data[i] >= 0 && data[i] <= Items.Count)
                Inventory[i] = Items[data[i]];
        }
    }

    private void Awake() {
        Instance = this;

        // set ID of available items, used for saving inventory to save file
        // this method will cause save loading issues if Items list is reordered in any way
        for (int i = 0; i < Items.Count; i++) {
            Items[i].ID = i;
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
