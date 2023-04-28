using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPoolManager : SingletonBase<ComponentPoolManager>, IComponentPoolManager
{
    protected Dictionary<string, ComponentPool> poolDic = new Dictionary<string, ComponentPool>();

    public void RegularClearPoolObj()
    {
        throw new NotImplementedException();
    }

    public void ClearPool()
    {
        poolDic?.Clear();
    }

    /// <summary>
    /// 从缓存池中取出组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="name">用于标识组件的名字</param>
    /// <param name="obj">组件依附的游戏物体</param>
    /// <param name="initCallback">初始化组件调用的委托(池里没有组件）</param>
    /// <param name="getFromPoolCallback">第一次以后取组件调用的委托</param>
    public void GetItem<T>(string name, Action initCallback, Action<T> getFromPoolCallback) where T : Component
    {
        if (poolDic.ContainsKey(name) && poolDic[name].itemPool.Count > 0)
        {
            getFromPoolCallback?.Invoke(poolDic[name].GetItem() as T);
        }
        else
        {
            initCallback?.Invoke();
        }       
    }

    public bool IsItemInPool(string keyName)
    {
        if (poolDic.ContainsKey(keyName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PushItem(string name, Component component)
    {
        if(poolDic.TryGetValue(name, out var pool))
        {
            pool.PushItem(component);
        }
        else
        {
            poolDic.Add(name, new ComponentPool(component));
        }
    }

    public void RemovePoolItem(string name)
    {
        if (poolDic.ContainsKey(name))
        {
            poolDic.Remove(name);
        }
    }
}
