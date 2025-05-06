using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSceneManager : CryptidUtils
{
    public static CSceneManager Instance;
    private List<Scene> loadedScenes = new();

    private void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void LoadScene(Scene scene) {
        // check if scene is already loaded
        if (SceneManager.GetSceneByName(scene.name).isLoaded) {
            LogWarn($"Failed to load scene \"{scene.name}\", scene already loaded!");
            return;
        }

        // unloading exclusive scenes
        if (scene.isExclusive) {
            if (scene.exclusiveTag != null) {
                int count = 0;
                foreach(Scene loadedScene in loadedScenes)
                    if (loadedScene.exclusiveTag == scene.exclusiveTag) {
                        UnloadScene(loadedScene);
                        count++;
                    }
                Log($"Found {count} loaded scenes with matching exclusive tag \"{scene.exclusiveTag}\"");
            } else {
                foreach (Scene loadedScene in loadedScenes)
                    if (loadedScene.type == Scene.sceneType.Exclusive)
                        UnloadScene(loadedScene);
            }
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
}
