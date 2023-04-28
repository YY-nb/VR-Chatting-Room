using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// ��ʾ�Լ����ⲿ�ɵ���
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
    /// �����Լ�
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
