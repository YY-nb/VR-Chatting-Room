using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �¼����� ����ģʽ���� ����������
/// 1.Dictionary
/// 2.ί��
/// 3.�۲������ģʽ
/// 4.����
/// </summary>
public class EventCenter : SingletonBase<EventCenter>
{
    //key ���� �¼������֣����磺�������������������ͨ�� �ȵȣ�
    //value ���� ��Ӧ���� ��������¼� ��Ӧ��ί�к�����
    private Dictionary<string, List<Delegate>> eventDic = new Dictionary<string, List<Delegate>>();

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListenerBase(string eventName, Delegate callback)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName].Add(callback);
        }
        else
        {
            eventDic.Add(eventName, new List<Delegate>() { callback});
        }
    }
    /// <summary>
    /// ��������Ҫ�������ݵ��¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener(string eventName, Action callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// ����¼�������1��������
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">׼�����������¼� ��ί�к���</param>
    public void AddListener<T>(string eventName, Action<T> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// ����¼�������2��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">׼�����������¼� ��ί�к���</param>
    public void AddListener<T1, T2>(string eventName, Action<T1, T2> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// ����¼�������3��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">׼�����������¼� ��ί�к���</param>
    public void AddListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// ����¼�������4��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">׼�����������¼� ��ί�к���</param>
    public void AddListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// �Ƴ��¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListenerBase(string eventName, Delegate callback)
    {
        if (eventDic.TryGetValue(eventName, out List<Delegate> eventList))
        {
            eventList.Remove(callback);
            //�¼��б�û���¼�ʱ�������¼���ֵ�Դ��ֵ����Ƴ�
            if (eventList.Count == 0)
            {
                eventDic.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// �Ƴ�����Ҫ�������¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener(string eventName, Action callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// �Ƴ���Ӧ���¼�������1��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveListener<T>(string eventName, Action<T> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// �Ƴ���Ӧ���¼�������2��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveListener<T1, T2>(string eventName, Action<T1, T2> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// �Ƴ���Ӧ���¼�������3��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// �Ƴ���Ӧ���¼�������4��������
    /// </summary>
    /// <param name="eventName">�¼�������</param>
    /// <param name="callback">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// �¼�����������Ҫ�����ģ�
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action)?.Invoke();
            }
        }
    }
    /// <summary>
    /// �¼�������1��������
    /// </summary>
    /// <param name="eventName">��һ�����ֵ��¼�������</param>
    public void TriggerEvent<T>(string eventName, T info)
    {
        if (eventDic.ContainsKey(eventName))
        {            
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T>)?.Invoke(info); 
            }
        }
    }
    /// <summary>
    /// �¼�������2��������
    /// </summary>
    /// <param name="eventName">��һ�����ֵ��¼�������</param>
    public void TriggerEvent<T1, T2>(string eventName, T1 info1, T2 info2)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2>)?.Invoke(info1, info2);
            }
        }
    }
    /// <summary>
    /// �¼�������3��������
    /// </summary>
    /// <param name="eventName">��һ�����ֵ��¼�������</param>
    public void TriggerEvent<T1, T2, T3>(string eventName, T1 info1, T2 info2, T3 info3)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2, T3>)?.Invoke(info1, info2, info3);
            }
        }
    }
    /// <summary>
    /// �¼�������4��������
    /// </summary>
    /// <param name="eventName">��һ�����ֵ��¼�������</param>
    public void TriggerEvent<T1, T2, T3, T4>(string eventName, T1 info1, T2 info2, T3 info3, T4 info4)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2, T3, T4>)?.Invoke(info1, info2, info3, info4);
            }
        }
    }
    /// <summary>
    /// ����¼�����
    /// ��Ҫ���� �����л�ʱ
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
