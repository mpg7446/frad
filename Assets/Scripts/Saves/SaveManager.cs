using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveManager : CryptidUtils
{
    public static SaveManager Instance;

    public string SaveFolder = Application.dataPath + "/Saves/";
    public List<Save> saves;
    public Save selectedSave;

    public GameObject SaveSlots;
    public GameObject SaveButton;

    private void Awake() {
        Instance = this;

        string[] data = Directory.GetFiles(SaveFolder, "*.json");
        saves = new List<Save>();

        foreach (string file in data) {
            string trimmed = file.Replace(SaveFolder, "").Replace(".json","");

            GameObject newButton = GameObject.Instantiate(SaveButton);
            newButton.transform.SetParent(SaveSlots.transform);
            newButton.GetComponentInChildren<TMP_Text>().text = trimmed;
            
            Save newSave = newButton.GetComponent<Save>();
            newSave.saveID = trimmed;
            saves.Add(newSave);
        }
    }

    public void SelectSave(Save save) {
        selectedSave = save;
    }
    public void SelectSave(int id) {
        if (id < 0 || id > saves.Count)
            return;

        selectedSave = saves[id];
    }
}
