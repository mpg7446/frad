using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save : CryptidUtils
{
    // TODO - look into PlayerPrefs as a possible data storage alternative to JsonUtility

    public string saveID;
    private string filename = Application.dataPath + "/Saves/";

    private void Start() {
        if (SaveManager.Instance != null)
            filename = SaveManager.Instance.SaveFolder;
    }

    [ContextMenu("Write SaveState Data")]
    public void WriteSave() {

        // save updated data
        SaveData data = new() {
            score = GameManager.Instance.comScore,
            items = InventoryManager.Instance.GetInventory()
        };
        
        // write data
        string json = JsonUtility.ToJson(data, true);
        TextWriter writer = new StreamWriter(filename + saveID + ".json", false);
        writer.Write(json);
        writer.Close();
    }
    [ContextMenu("Read SaveState Data")]
    public void ReadSave() {
        // read data
        if (!File.Exists(filename + saveID + ".json"))
            return;

        TextReader reader = new StreamReader(filename + saveID + ".json");
        SaveData data = JsonUtility.FromJson<SaveData>(reader.ReadToEnd());

        // apply read data
        GameManager.Instance.comScore = data.score;
        InventoryManager.Instance.SetInventory(data.items);
    }
}


[System.Serializable]
public class SaveData {
    public int score;
    public string[] items;
}
