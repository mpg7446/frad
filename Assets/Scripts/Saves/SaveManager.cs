using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveManager : CryptidUtils
{
    public static SaveManager Instance;

    public string SaveFolder;
    public List<Save> saves;
    public Save selectedSave;

    [Tooltip("Parent Object to instantiate button prefabs to")]
    public GameObject SaveSlots;
    [Tooltip("Save Slot Button prefab")]
    public GameObject SaveButton;

    private void Awake() {
        Instance = this;

        if (string.IsNullOrEmpty(SaveFolder))
            SaveFolder = Application.persistentDataPath + "/Saves/";

        string[] data = Directory.GetFiles(SaveFolder, "*.json");
        saves = new List<Save>();

        foreach (string file in data) {
            string trimmed = file.Replace(SaveFolder, "").Replace(".json","");
            NewSaveButton(trimmed);
        }
    }

    private void NewSaveButton(string name) {
        GameObject newButton = Instantiate(SaveButton);
        newButton.transform.SetParent(SaveSlots.transform);

        Save newSave = newButton.GetComponent<Save>();
        newSave.SetData(name);
        saves.Add(newSave);
    }

    public void SelectSave(Save save) {
        selectedSave = save;
    }

    [ContextMenu("Reset Save Path")]
    public void ResetSavePath() => SaveFolder = Application.persistentDataPath + "/Saves/";
}
