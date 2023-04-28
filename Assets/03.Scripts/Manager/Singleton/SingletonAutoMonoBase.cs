using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoMonoBase<T> : MonoBehaviour where T:MonoBehaviour
{
    private static bool isApplicationQuit;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (isApplicationQuit)
            {
                return instance;
            }
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                DontDestroyOnLoad(obj);//这行代码根据实际需求决定是否写在基类中
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
    protected virtual void OnDestroy()
    {
        isApplicationQuit = true;
    }
}
