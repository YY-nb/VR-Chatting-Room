using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// UI层级
/// </summary>
public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System
}
/// <summary>
/// UI管理器 （适用于场景中只有一个Canvas）
/// 1.管理所有显示的面板
/// 2.提供给外部显示和隐藏等接口
/// </summary>
public class UIManager : SingletonBase<UIManager>
{
    private Dictionary<string, BaseUIPanel> panelDic = new Dictionary<string, BaseUIPanel>();

    private Transform bottom;
    private Transform middle;
    private Transform top;
    private Transform system;
    //记录UI的Canvas父对象
    public RectTransform canvas;
    public UIManager()
    {
        //创建canvas，过场景不移除
        GameObject obj = ResourceManager.Instance.Load<GameObject>(ResourceName.CanvasPath); //找到canvas
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);
        //找到各层，编辑器中的这四个物体名必须和nameof里的一样
        bottom = canvas.Find(nameof(bottom));
        middle = canvas.Find(nameof(middle));
        top = canvas.Find(nameof(top));
        system = canvas.Find(nameof(system));
        //创建EventSystem，过场景不移除
        obj =ResourceManager.Instance.Load<GameObject>(ResourceName.EventSystemPath);
        GameObject.DontDestroyOnLoad(obj);

    }
    /// <summary>
    /// 通过层级枚举得到对应层级的父对象
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
    /// 显示面板
    /// UI预设体存储的路径为UI/xxx，Addressables和Resources都一样
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后做的事</param>
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
                //把它作为Canvas子对象
                //并且设置它处在哪一层
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
                //把UI设置为所处层的子对象，设置相对位置，大小，和偏移
                obj.name = panelName;
                obj.transform.SetParent(father);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                (obj.transform as RectTransform).offsetMax = Vector2.zero;
                (obj.transform as RectTransform).offsetMin = Vector2.zero;

                //得到预设体身上的面板脚本
                T panel = obj.GetComponent<T>();
                if (callBack != null)
                {
                    panel.Show(() => callBack?.Invoke(panel));
                }
                else
                {
                    panel.Show();
                }                            
                //把面板存起来
                panelDic.Add(panelName, panel);
            });
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">面板名</param>
    /// <param name="callback">隐藏面板完毕之后的委托</param>
    public void HidePanel(string panelName, Action callback = null)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide(callback);
        }
    }
    /// <summary>
    /// 销毁面板
    /// </summary>
    /// <param name="panelName">面板名</param>
    /// <param name="callback">销毁面板之后的委托</param>
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
    /// 销毁面板时的回调
    /// </summary>
    /// <param name="panelName"></param>
    private void OnDestroyPanel(string panelName)
    {
        GameObject.Destroy(panelDic[panelName].gameObject);
        panelDic.Remove(panelName);
    }
    /// <summary>
    /// 得到某一个已经显示的面板，方便外部使用
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
    /// 清空字典
    /// </summary>
    public void ClearDic()
    {
        panelDic?.Clear();
    }
    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
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
