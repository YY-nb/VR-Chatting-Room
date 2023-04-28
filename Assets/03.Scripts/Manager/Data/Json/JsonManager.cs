using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;

/// <summary>
/// 序列化和反序列化Json时  使用的是哪种方案
/// LitJson和Newtonsoft可能需要.NET 4.x
/// </summary>
public enum JsonType
{
    JsonUtlity,
    LitJson,
    NewtonsoftJson
}

/// <summary>
/// Json数据管理类 主要用于进行 Json的序列化存储到硬盘 和 反序列化从硬盘中读取到内存中
/// </summary>
public class JsonManager : SingletonBase<JsonManager>
{
    /// <summary>
    /// 存储Json数据 序列化
    /// 将类转换成Json字符串写入到文件中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public void SaveData<T>(T data, string fileName, bool isSaveInPersistentDataPath = true, JsonType type = JsonType.NewtonsoftJson) where T : new()
    {
        string path = null;
        //确定存储路径
        if (isSaveInPersistentDataPath)
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        else
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".json";
        }
        //序列化 得到Json字符串
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                //jsonStr = JsonMapper.ToJson(data);
                break;
            case JsonType.NewtonsoftJson:
                jsonStr = JsonConvert.SerializeObject(data);
                break;
        }
        //把序列化的Json字符串 存储到指定路径的文件中
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// 读取指定文件中的 Json数据 反序列化
    /// 找到Json文件，将文本转化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public T LoadData<T>(string fileName, JsonType type = JsonType.NewtonsoftJson) where T : new()
    {
        string jsonStr = null;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //确定从哪个路径读取
            //首先先判断 默认数据文件夹中是否有我们想要的数据 如果有 就从中获取
            string path = Application.streamingAssetsPath + "/" + fileName + ".json";
            //先判断 是否存在这个文件
            //如果不存在默认文件 就从 读写文件夹中去寻找
            if (!File.Exists(path))
            {
                path = Application.persistentDataPath + "/" + fileName + ".json";
            }

            //如果读写文件夹中都还没有 那就返回一个默认对象
            if (!File.Exists(path))
            {
                return new T();
            }

            
            //进行反序列化

            jsonStr = File.ReadAllText(path);

        } 
        //数据对象
        T data = default(T); //代表T的默认值
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                //data = JsonMapper.ToObject<T>(jsonStr);
                break;
            case JsonType.NewtonsoftJson:
                data = JsonConvert.DeserializeObject<T>(jsonStr);
                break;
        }
        //把对象返回出去
        return data;
    }
    private T JsonToObj<T>(string jsonStr, JsonType type = JsonType.NewtonsoftJson) where T:new()
    {
        T data = default(T); //代表T的默认值
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                //data = JsonMapper.ToObject<T>(jsonStr);
                break;
            case JsonType.NewtonsoftJson:
                data = JsonConvert.DeserializeObject<T>(jsonStr);
                break;
        }
        //把对象返回出去
        return data;
    }
    public void LoadDataFromAB<T>(string fileName,Action<T> callback, JsonType type = JsonType.NewtonsoftJson) where T : new()
    {
        ResourceManager.Instance.LoadAsync<TextAsset>(fileName, (textAsset) =>
        {
            T data = JsonToObj<T>(textAsset.text, type);
            callback?.Invoke(data);
        });
    }
    public void DeleteData(string fileName)
    {
        //确定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
        {
            Debug.Log($"找不到{path}，或还未保存存档点");
            return;
        }
        File.Delete(path);
    }
}

