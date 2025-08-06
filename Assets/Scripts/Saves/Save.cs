using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save : CryptidUtils
{
    // PLEASE WORK ON THIS
    // i have no idea how im structuring this, whether to make it indivudual save locations(???)
    // have one class that loads the data upon selecting which save

    // TODO - look into PlayerPrefs as a possible data storage alternative to JsonUtility

    public string saveID;
    private string filename = Application.dataPath + "/Saves/";
    //public int[] items;

    private void Start() {
        if (SaveManager.Instance != null)
            filename = SaveManager.Instance.SaveFolder;

        filename += saveID + ".json";
    }

    [ContextMenu("Write SaveState Data")]
    public void WriteSave() {
        // get updated data
        //items = new int[InventoryManager.Instance.Inventory.Length];
        //for (int i = 0; i < items.Length; i++) {
        //    if (InventoryManager.Instance.Inventory[i] == null)
        //        items[i] = InventoryManager.Instance.Inventory[i].ID;
        //    else
        //        items[i] = -1;
        //}

        // save updated data
        SaveData data = new() {
            score = GameManager.Instance.comScore,
            items = InventoryManager.Instance.GetInventory()
        };
        
        // write data
        string json = JsonUtility.ToJson(data, true);
        TextWriter writer = new StreamWriter(filename, false);
        writer.Write(json);
        writer.Close();
    }
    [ContextMenu("Read SaveState Data")]
    public void ReadSave() {
        // read data
        if (!File.Exists(filename))
            return;

        TextReader reader = new StreamReader(filename);
        SaveData data = JsonUtility.FromJson<SaveData>(reader.ReadToEnd());

        // apply read data
        GameManager.Instance.comScore = data.score;
        InventoryManager.Instance.SetInventory(data.items);
    }
}


[System.Serializable]
public class SaveData {
    public int score;
    public int[] items;
}
