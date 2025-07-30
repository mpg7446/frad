using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save : CryptidUtils
{
    // PLEASE WORK ON THIS
    // i have no idea how im structuring this, whether to make it indivudual save locations(???)
    // have one class that loads the data upon selecting which save

    public static Save Instance;
    private readonly string filename = Application.dataPath + "/Saves.json";
    public int score;

    private void Start() {
        Instance = this;
    }

    [ContextMenu("Save Save Data")]
    public void WriteSave() {
        // get old data


        // save updated data
        SaveData data = new() {
            score1 = this.score,
            score2 = this.score,
            score3 = this.score
        };
        
        // write data
        string json = JsonUtility.ToJson(data);
        TextWriter writer = new StreamWriter(filename, false);
        writer.Write(json);
        writer.Close();
    }
}

[System.Serializable]
public class SaveData {
    public int score1;
    public int score2;
    public int score3;
}
