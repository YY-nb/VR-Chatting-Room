using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// ��ʾ�Լ����ⲿ�ɵ���
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
    /// �����Լ�
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
