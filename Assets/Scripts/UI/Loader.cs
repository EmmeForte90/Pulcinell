using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private class LoadingMonoBehaviour : MonoBehaviour { }
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu

    }

    private static Action onLoaderCallBack;
    private static AsyncOperation loadingAsyncOperation;
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.Loading.ToString());

        onLoaderCallBack = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            LoadSceneAsync(scene);
        };
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        loadingAsyncOperation  = SceneManager.LoadSceneAsync(scene.ToString());
   
        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }

    }

    public static float GetLoadingProgress()
    {
        if(loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        } else
        { return 1f; }
    }


    public static void LoaderCallBack()
    {
        if(onLoaderCallBack != null)
        {
            onLoaderCallBack();
            onLoaderCallBack = null;
        }
    }

}
