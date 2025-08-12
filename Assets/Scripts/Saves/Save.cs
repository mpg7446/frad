using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Save : CryptidUtils
{
    // TODO - look into PlayerPrefs as a possible data storage alternative to JsonUtility

    [SerializeField] private string saveID;
    private string filePath;

    [SerializeField] private TMP_Text titleField;
    [SerializeField] private TMP_Text descriptionField;

    // data for user save identification - not loaded into managers until save is confirmed
    private int score;
    private string[] items;
    private float time;
    private int runs;

    private void Awake() {
        if (SaveManager.Instance != null)
            filePath = SaveManager.Instance.SaveFolder;
        else
            filePath = Application.persistentDataPath + "/Saves/";
    }

    public void SetData(string name) {
        saveID = name;

        if (titleField == null || descriptionField == null) // not all instances will contain title and description, such as "New Save" button
            return;

        // set visuals for button
        ReadSave();
        titleField.text = $"\"{name}\"";
        descriptionField.text = $"Score: {score} | Runs: {runs} | Time: {time}";
    }

    public void Select() {
        SaveManager.Instance.SelectSave(this);
        InventoryManager.Instance.SetInventory(items);

        GameManager.Instance.comScore = score;
        GameManager.Instance.comTime = time;
        GameManager.Instance.comRuns = runs;
        GameManager.Instance.StartGame();
    }

    [ContextMenu("Write SaveState Data")]
    public void WriteSave() {

        // save updated data
        SaveData data = new() {
            score = GameManager.Instance.comScore,
            items = InventoryManager.Instance.GetInventory(),
            time = GameManager.Instance.comTime,
            runs = GameManager.Instance.comRuns
        };
        
        // write data
        string json = JsonUtility.ToJson(data, true);
        TextWriter writer = new StreamWriter(filePath + saveID + ".json", false);
        writer.Write(json);
        writer.Close();
    }
    [ContextMenu("Read SaveState Data")]
    public void ReadSave() {
        // read data
        if (!File.Exists(filePath + saveID + ".json")) {
            LogErr($"Unable to find file \"{saveID}\" ({filePath + saveID + ".json"})");
            return;
        }

        TextReader reader = new StreamReader(filePath + saveID + ".json");
        SaveData data = JsonUtility.FromJson<SaveData>(reader.ReadToEnd());

        // apply read data
        score = data.score;
        items = data.items;
        time = data.time;
        runs = data.runs;
    }
    [ContextMenu("Destroy Save Data")]
    public void DestroySave() {

    }
}


[System.Serializable]
public class SaveData {
    public int score;
    public string[] items;
    public float time;
    public int runs;
}
