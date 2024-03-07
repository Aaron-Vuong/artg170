using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// https://www.youtube.com/watch?v=3I5d2rUJ0pE&ab_channel=CodeMonkey
public class SceneChangeManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private Logger _logger;

    [Header("Scene State")]
    [SerializeField] private Dictionary<Scene, SceneState> _sceneStates = new Dictionary<Scene, SceneState>();
    private class LoadingMonoBehavior: MonoBehaviour { }
    public enum Scene {
        MainGameScene,
        Loading,
        MainMenuScene,
        HouseLevel,
        Level2,
        Level3,
        GameOver
    }

    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;
    private static Scene getCurrentScene()
    {
        // Convert a UnityEngine.SceneManagement.Scene to our own Scene enum
        // TODO: Probably a better way to organize scenes than this.
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        string[] sceneNames = System.Enum.GetNames(typeof(Scene));

        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (scene.name == sceneNames[i])
            {
                Debug.Log($"Current Scene is {sceneNames[i]}");
                return (Scene)System.Enum.Parse(typeof(Scene), sceneNames[i]);
            }
        }
        Debug.Log("Scene NOT FOUND!");
        return Scene.MainMenuScene;
    }
    private static void SaveSceneState()
    {
        Scene scene = getCurrentScene();
        // Save off the positions of all objects labelled with Pickup or Mob.
        return;
    }

    private static void ApplySceneState(Scene scene)
    {
        // If there is no SceneState for this scene, create one.
        return;
    }
    public static void Load(Scene scene)
    {
        // Save off the current SceneState.
        SaveSceneState();
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading GameObject");
            loadingGameObject.AddComponent<LoadingMonoBehavior>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
        // Apply Scene State to the scene.
        ApplySceneState(scene);
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        } 
        else
        {
            return 1f;
        }
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }


    private void Start()
    {
        _logger.Log("Loading SceneManager!");
    }
}
