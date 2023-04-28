using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : ObjectPoolBase<GameObject>
{
    //某一类缓存池游戏物体的父物体，而所有这种父物体还会有一个根物体
    public GameObject fatherObj;
    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="poolObj">池子里存储的某一类游戏物体</param>
    /// <param name="gameObjectRoot">所有类型的缓存池游戏物体的根物体</param>
    public GameObjectPool(GameObject poolObj, GameObject gameObjectRoot)
    {
        //给一类对象创建一个父对象，并且把父对象作为pool根对象的子物体
        fatherObj = new GameObject(poolObj.name);
        fatherObj.transform.parent = gameObjectRoot.transform;
        itemPool = new SingleLinkedList<GameObject>();
        PushItem(poolObj);
    }
    public override GameObject GetItem()
    {
        var obj = itemPool.First.Value;
        itemPool.DeleteFirst();
        obj.transform.SetParent(null,false);
        obj.SetActive(true);
        return obj;
    }
    public override void PushItem(GameObject item)
    {
        itemPool.AddLast(item);
        item.SetActive(false);
        item.transform.SetParent(fatherObj.transform,false);
    }
}
