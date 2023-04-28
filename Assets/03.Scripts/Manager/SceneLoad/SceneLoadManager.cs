using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 场景加载管理
/// </summary>
public class SceneLoadManager : SingletonBase<SceneLoadManager>
{
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="onFinish"></param>
    /// <param name="resourceLoadWay"></param>
    public void LoadSceneAsync(string sceneName, Action onFinish, ResourceLoadWay resourceLoadWay=ResourceLoadWay.SceneManagement)
    {
        if (resourceLoadWay == ResourceLoadWay.Addressables)
        {
            AddressablesManager.Instance.LoadSceneAsync(sceneName, ()=>
            {
                onFinish?.Invoke();
            });
        }
        else if(resourceLoadWay == ResourceLoadWay.SceneManagement)
        {
            MonoManager.Instance.StartCoroutine(I_LoadSceneAsync(sceneName, () =>
            {
                onFinish?.Invoke();
            }));
        }
    }
    private IEnumerator I_LoadSceneAsync(string name, Action onFinish)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        while (!async.isDone)
        {
            //分发加载场景事件：一般是UI加载场景的动画
            //这里触发什么事件按需求而定，名字待定

            yield return async.progress;
        }
        onFinish?.Invoke();
        yield return null; 
    }
    
}

