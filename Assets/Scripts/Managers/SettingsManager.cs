using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using UnityEngine;
using Palmmedia.ReportGenerator.Core.Common;
using Unity.VisualScripting;

public class SettingsManager : CryptidUtils
{
    public static SettingsManager Instance;
    private static readonly string filename = Application.dataPath + "/Settings.json";

    public static float s_sensitivity = 1;
    public static float s_maxVignette = 1;
    public static float s_pixelationIntensity = 1.534f;
    public static float s_ditheringScale = 0;

    public static bool hasChanged = false;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this, "Instance already exists");

        ReadSettings();
    }

    private void OnDestroy() => WriteSettings();

    [ContextMenu("Load Settings")]
    public void ReadSettings() {
        if (!File.Exists(filename)) {
            WriteSettings();
            return;
        }
        TextReader reader = new StreamReader(filename);
        SettingsData data = JsonUtility.FromJson<SettingsData>(reader.ReadLine());
        s_sensitivity = data.sensitivity;
        s_maxVignette = data.maxVignette;
        s_pixelationIntensity = data.pixelationIntensity;
        s_ditheringScale = data.ditheringScale;
    }

    [ContextMenu("Save Settings")]
    public void WriteSettings() {
        SettingsData data = new() {
            sensitivity = s_sensitivity,
            maxVignette = s_maxVignette,
            pixelationIntensity = s_pixelationIntensity,
            ditheringScale = s_ditheringScale
        };
        string json = JsonUtility.ToJson(data);

        TextWriter writer = new StreamWriter(filename, false);
        writer.WriteLine(json);
        writer.Close();
    }

    public void ChangeSensitivity(float value) {
        s_sensitivity = Mathf.Clamp(value, 0.01f, 4f);
        hasChanged = true;
    }
    public void ChangeVignette(float value) {
        s_maxVignette = value;
        hasChanged = true;
    }
    public void ChangePixelation(float value) {
        s_maxVignette = Mathf.Clamp(value, 0.01f, 2);
        hasChanged = true;
    }
    public void ChangeDithering(float value) {
        s_maxVignette = Mathf.Clamp(value, 0.01f, 2);
        hasChanged = true;
    }
}

[System.Serializable]
public class SettingsData {
    public float sensitivity;
    public float maxVignette;
    public float pixelationIntensity;
    public float ditheringScale;
}
