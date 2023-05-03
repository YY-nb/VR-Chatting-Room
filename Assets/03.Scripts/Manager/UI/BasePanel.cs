using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 显示自己，外部可调用
    /// </summary>
    public virtual void Show(Action onFinish = null, Action onBegin = null)
    {
        onBegin?.Invoke();
        onFinish?.Invoke();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true); 
        }

    }
    /// <summary>
    /// 隐藏自己
    /// </summary>
    public virtual void Hide(Action onFinish = null, Action onBegin = null)
    {
        onBegin?.Invoke();   
        onFinish?.Invoke();
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

    }
}
