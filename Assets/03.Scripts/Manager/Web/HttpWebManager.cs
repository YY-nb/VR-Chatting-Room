using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
/// <summary>
/// Http请求和响应的管理器
/// Json转换基于Newtonsoft.Json
/// </summary>
public class HttpWebManager : SingletonBase<HttpWebManager>
{
    private string TokenHeader = "Authorization";
    public string LoginToken { get; set; }
   
    /// <summary>
    /// 请求参数为Json格式的Post请求
    /// </summary>
    /// <typeparam name="E">请求参数对象类型</typeparam>
    /// <typeparam name="T">响应对象中data的类型</typeparam>
    /// <param name="url"></param>
    /// <param name="request"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <param name="contentType"></param>
    public void PostJsonBody<E,T>(string url, E request, Action<Response<T>> onSuccess = null, bool isTokenNeed=true, Action onFailure = null, string contentType = "application/json;charset=utf-8") where T : new()
    {
        MonoManager.Instance.StartCoroutine(I_PostJsonBody(url, request, onSuccess, isTokenNeed, onFailure, contentType));
    }
    private IEnumerator I_PostJsonBody<E,T>(string url, E request, Action<Response<T>> onSuccess = null, bool isTokenNeed = true, Action onFailure=null, string contentType = "application/json;charset=utf-8") where T : new()   
    {
        string content = JsonConvert.SerializeObject(request);
        UnityWebRequest www = UnityWebRequest.Post(url, content);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(content);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        if(isTokenNeed)
            www.SetRequestHeader(TokenHeader, LoginToken);
        www.SetRequestHeader("Content-Type", contentType);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
#if UNITY_EDITOR
            Debug.Log("Post on" + url + "Error" + www.responseCode);
#endif
            onFailure?.Invoke();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"响应结果:{www.downloadHandler.text}");
#endif
            var value = JsonConvert.DeserializeObject<Response<T>>(www.downloadHandler.text);
            onSuccess?.Invoke(value);
        }

    }
    /// <summary>
    /// 请求参数为Form格式的Post请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="formDic"></param>
    /// <param name="onSuccess"></param>
    /// <param name="isTokenNeed"></param>
    /// <param name="onFailure"></param>
    public  void PostForm<T>(string url, Dictionary<string, string> formDic, Action<Response<T>> onSuccess=null, bool isTokenNeed=true, Action onFailure=null) where T : class
    {
        MonoManager.Instance.StartCoroutine(I_PostForm(url, formDic, onSuccess, isTokenNeed, onFailure));
    }

    private  IEnumerator I_PostForm<T>(string url, Dictionary<string, string> formDic, Action<Response<T>> onSuccess = null, bool isTokenNeed = true, Action onFailure = null) where T : class
    {
        WWWForm form = new WWWForm();
        foreach (var data in formDic)
        {
            form.AddField(data.Key, data.Value);
        }
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        if (isTokenNeed)
            www.SetRequestHeader(TokenHeader, LoginToken);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
#if UNITY_EDITOR
            Debug.Log("Post on" + url + "Error" + www.responseCode);
#endif
            onFailure?.Invoke();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"响应结果:{www.downloadHandler.text}");
#endif
            var value = JsonConvert.DeserializeObject<Response<T>>(www.downloadHandler.text);
            onSuccess?.Invoke(value);
        }
    }
    /// <summary>
    /// 下载网络上的图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    public void GetPictureByUrl(string url, Action<Texture2D> onSuccess, Action onFailure = null)
    {
        MonoManager.Instance.StartCoroutine(I_GetPictureByUrl(url, onSuccess, onFailure));
    }
    private  IEnumerator I_GetPictureByUrl(string url, Action<Texture2D> onSuccess, Action onFailure=null)
    {
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerTexture(true);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //image.sprite = sprite;           
            onSuccess?.Invoke(texture);
        }
        else
        {
            onFailure?.Invoke();
        }
    }
    /// <summary>
    /// Get请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="onSuccess"></param>
    /// <param name="isTokenNeed"></param>
    /// <param name="onFailure"></param>
    public void Get<T>(string url, Action<Response<T>> onSuccess=null, bool isTokenNeed=true, Action onFailure=null)
    {
        MonoManager.Instance.StartCoroutine(I_Get(url, onSuccess, isTokenNeed, onFailure));
    }
     private IEnumerator I_Get<T>(string url, Action<Response<T>> onSuccess = null, bool isTokenNeed = true, Action onFailure=null)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        if (isTokenNeed)
            www.SetRequestHeader(TokenHeader, LoginToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
#if UNITY_EDITOR
            Debug.Log("Post on" + url + "Error" + www.responseCode);
#endif
            onFailure?.Invoke();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"响应结果:{www.downloadHandler.text}");
#endif
            var value = JsonConvert.DeserializeObject<Response<T>>(www.downloadHandler.text);
            onSuccess?.Invoke(value);
        }

    }
}
