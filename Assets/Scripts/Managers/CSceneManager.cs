using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSceneManager : CryptidUtils
{
    public static CSceneManager Instance;
    private readonly List<Scene> loadedScenes = new();
    public bool IsPrimaryManager = false;

    private void Start() {
        if (!IsPrimaryManager && Instance == null) {
            SceneManager.LoadScene("GameManagers", LoadSceneMode.Single);
            Destroy(this);
            return;
        }

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void LoadScene(Scene scene) {
        // check if scene is already loaded
        if (SceneManager.GetSceneByName(scene.name).isLoaded) {
            LogWarn($"Failed to load scene \"{scene.name}\", scene already loaded!");
            if (!loadedScenes.Contains(scene))
                loadedScenes.Add(scene);
            return;
        }

        // unloading exclusive scenes
        if (scene.isExclusive && loadedScenes.Count > 0) {
            UnloadExclusives(scene.exclusiveTag);
            /*List<Scene> unload = new();
            if (scene.exclusiveTag != null) {
                int count = 0;
                foreach (Scene loadedScene in loadedScenes)
                    if (loadedScene.exclusiveTag.Equals(scene.exclusiveTag)) {
                        unload.Add(loadedScene);
                        count++;
                    }
                Log($"Found {count} loaded scenes with matching exclusive tag \"{scene.exclusiveTag}\"");
            }
            else {
                foreach (Scene loadedScene in loadedScenes)
                    if (loadedScene.type == Scene.SceneType.Exclusive)
                        unload.Add(loadedScene);
            }

            if (unload.Count > 0)
                foreach (Scene loadedScene in unload)
                    UnloadScene(loadedScene);*/
        }

        // load new scene
        loadedScenes.Add(scene);
        SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
    }

    public void UnloadScene(Scene scene) {
        if (loadedScenes.Contains(scene))
            loadedScenes.Remove(scene);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(scene.name).buildIndex);
    }

    public void UnloadExclusives(string exclusiveTag = null) {
        List<Scene> unload = new();
        if (exclusiveTag != null) {
            int count = 0;
            foreach (Scene loadedScene in loadedScenes)
                if (loadedScene.exclusiveTag.Equals(exclusiveTag)) {
                    unload.Add(loadedScene);
                    count++;
                }
            Log($"Found {count} loaded scenes with matching exclusive tag \"{exclusiveTag}\"");
        }
        else {
            foreach (Scene loadedScene in loadedScenes)
                if (loadedScene.type == Scene.SceneType.Exclusive)
                    unload.Add(loadedScene);
        }

        if (unload.Count > 0)
            foreach (Scene loadedScene in unload)
                UnloadScene(loadedScene);
    }
}
