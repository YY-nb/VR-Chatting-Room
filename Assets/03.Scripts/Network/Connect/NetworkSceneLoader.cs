using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneLoader : SingletonAutoMonoBase<NetworkSceneLoader>
{    
    private string sceneNameToBeLoaded;
    private bool isSceneSynched = false;
    public void LoadScene(string _sceneName, bool _isSceneSynched, Action callback= null)
    {
        sceneNameToBeLoaded = _sceneName;
        isSceneSynched = _isSceneSynched;
        StartCoroutine(InitializeSceneLoading(callback));
    }



    IEnumerator InitializeSceneLoading(Action callback)
    {
        ClearUtil.ClearDataInManagers();
        //First, we load the Loading scene
        yield return SceneManager.LoadSceneAsync("Loading Scene");

        //Load the actual scene
        StartCoroutine(ShowOverlayAndLoad(callback));
    }

    /// <summary>
    /// If isSceneSynch is true, this coroutine loads the scene with PhotonNetwork.LoadLevel method.
    /// PhotonNetwork.LoadLevel is used when synchronizing the scenes.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator ShowOverlayAndLoad(Action callback)
    {
        //Waiting some seconds to prevent "pop" to new scene
        yield return new WaitForSeconds(4f);

        if (isSceneSynched)
        {
            //If Scene should be loaded as a Multiplayer scene, use PhotonNetwork.LoadLevel
            PhotonNetwork.LoadLevel(sceneNameToBeLoaded);
            while(PhotonNetwork.LevelLoadingProgress < 1)
            {
                yield return null;
            }
            callback?.Invoke();
        }
        else
        {
            //If it is a local scene loading, load the scene locally.
            //Load Scene and wait until complete
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneNameToBeLoaded);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            callback?.Invoke();
            yield return null;
        }
    }
}
