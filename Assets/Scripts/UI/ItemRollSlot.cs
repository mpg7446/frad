using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemRollSlot : CryptidUtils {
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    public int id;

    private void Start() {
        InventoryManager.Instance.RegisterRollSlot(this);

        if (id < 0)
            id = 0;
    }

    public void SetData(string name, string description) {
        this.name.text = name;
        this.description.text = description;
    }

    public void OnClick() {
        InventoryManager.Instance.SelectItem(id);
        MenuManager.Instance.OpenMain();
    }
}
