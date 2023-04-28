using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 显示自己，外部可调用
    /// </summary>
    public virtual void Show(Action callback = null)
    {
        
        callback?.Invoke();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true); 
        }

    }
    /// <summary>
    /// 隐藏自己
    /// </summary>
    public virtual void Hide(Action callback = null)
    {
        callback?.Invoke();
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

    }
}
