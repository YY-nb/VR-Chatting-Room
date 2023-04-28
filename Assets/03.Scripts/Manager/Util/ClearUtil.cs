using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClearUtil 
{
    /// <summary>
    /// 进行一些清理操作，一般在移除场景时调用
    /// </summary>
   public static void ClearDataInManagers()
    {
        UI3DManager.Instance.ClearDic();
        EventCenter.Instance.Clear();
        GameObjectPoolManager.Instance.ClearPool();
        ComponentPoolManager.Instance.ClearPool();
        Scheduler.Instance.Clear();
    }

    public static void ClearDataInManagersWithAB()
    {
        ClearDataInManagers();
        AddressablesManager.Instance.Clear();
    }
}
