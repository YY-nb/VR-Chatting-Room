using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// UI�㼶
/// </summary>
public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System
}
/// <summary>
/// UI������ �������ڳ�����ֻ��һ��Canvas��
/// 1.����������ʾ�����
/// 2.�ṩ���ⲿ��ʾ�����صȽӿ�
/// </summary>
public class UIManager : SingletonBase<UIManager>
{
    private Dictionary<string, BaseUIPanel> panelDic = new Dictionary<string, BaseUIPanel>();

    private Transform bottom;
    private Transform middle;
    private Transform top;
    private Transform system;
    //��¼UI��Canvas������
    public RectTransform canvas;
    public UIManager()
    {
        //����canvas�����������Ƴ�
        GameObject obj = ResourceManager.Instance.Load<GameObject>(ResourceName.CanvasPath); //�ҵ�canvas
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);
        //�ҵ����㣬�༭���е����ĸ������������nameof���һ��
        bottom = canvas.Find(nameof(bottom));
        middle = canvas.Find(nameof(middle));
        top = canvas.Find(nameof(top));
        system = canvas.Find(nameof(system));
        //����EventSystem�����������Ƴ�
        obj =ResourceManager.Instance.Load<GameObject>(ResourceName.EventSystemPath);
        GameObject.DontDestroyOnLoad(obj);

    }
    /// <summary>
    /// ͨ���㼶ö�ٵõ���Ӧ�㼶�ĸ�����
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bottom:
                return bottom;
            case E_UI_Layer.Middle:
                return middle;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;
        }
        return null;
    }
    /// <summary>
    /// ��ʾ���
    /// UIԤ����洢��·��ΪUI/xxx��Addressables��Resources��һ��
    /// </summary>
    /// <typeparam name="T">���ű�����</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="layer">��ʾ����һ��</param>
    /// <param name="callBack">�����Ԥ���崴���ɹ���������</param>
    public void ShowPanel<T>(string panelName,  Action<T> callBack = null, E_UI_Layer layer = E_UI_Layer.Middle) where T : BaseUIPanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            if (callBack != null)
            {
                panelDic[panelName].Show(() => callBack?.Invoke(panelDic[panelName] as T));
            }
            else
            {
                panelDic[panelName].Show();
            }
            return;
        }
        ResourceManager.Instance.LoadAsync<GameObject>(ResourceName.UIBasePath + panelName,
            (obj) =>
            {
                //������ΪCanvas�Ӷ���
                //����������������һ��
                Transform father = bottom;
                switch (layer)
                {
                    case E_UI_Layer.Middle:
                        father = middle;
                        break;
                    case E_UI_Layer.Top:
                        father = top;
                        break;
                    case E_UI_Layer.System:
                        father = system;
                        break;
                }
                //��UI����Ϊ��������Ӷ����������λ�ã���С����ƫ��
                obj.name = panelName;
                obj.transform.SetParent(father);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                (obj.transform as RectTransform).offsetMax = Vector2.zero;
                (obj.transform as RectTransform).offsetMin = Vector2.zero;

                //�õ�Ԥ�������ϵ����ű�
                T panel = obj.GetComponent<T>();
                if (callBack != null)
                {
                    panel.Show(() => callBack?.Invoke(panel));
                }
                else
                {
                    panel.Show();
                }                            
                //����������
                panelDic.Add(panelName, panel);
            });
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">�����</param>
    /// <param name="callback">����������֮���ί��</param>
    public void HidePanel(string panelName, Action callback = null)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide(callback);
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">�����</param>
    /// <param name="callback">�������֮���ί��</param>
    public void DestroyPanel(string panelName, Action callback = null)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide(() =>
            {
                OnDestroyPanel(panelName);
                callback?.Invoke();
            });
        }
    }

    /// <summary>
    /// �������ʱ�Ļص�
    /// </summary>
    /// <param name="panelName"></param>
    private void OnDestroyPanel(string panelName)
    {
        GameObject.Destroy(panelDic[panelName].gameObject);
        panelDic.Remove(panelName);
    }
    /// <summary>
    /// �õ�ĳһ���Ѿ���ʾ����壬�����ⲿʹ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName) where T : BaseUIPanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }
    /// <summary>
    /// ����ֵ�
    /// </summary>
    public void ClearDic()
    {
        panelDic?.Clear();
    }
    /// <summary>
    /// ���ؼ�����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callBack">�¼�����Ӧ����</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        trigger.triggers.Add(entry);
    }

}
