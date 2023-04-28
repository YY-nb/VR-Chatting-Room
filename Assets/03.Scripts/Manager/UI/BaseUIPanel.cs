using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// ������
/// ����ͨ����������ҵ��Լ���������е�UI�ؼ�����(�ű�)��Ȼ����������ʵ������߼�
/// �ṩ��ʾ�����ؽӿ�
/// </summary>
public class BaseUIPanel : BasePanel
{
    /// <summary>
    /// �ֵ��¼UI������е�����UI���(���е�UI������̳���UIBehaviour),stringΪgameObject����
    /// </summary>
    private Dictionary<string, List<UIBehaviour>> uiDic = new Dictionary<string, List<UIBehaviour>>();
    protected virtual void Awake()
    {
        //�� Panel ������UI�����ӵ��ֵ���
        //ʾ���÷�����Ŀ��û�г��ֵ�������Բ���Ѱ�ң�Ҳ�����������Awake�����
        AddChildrenUIComponentsToDic<Button>();
        AddChildrenUIComponentsToDic<TextMeshProUGUI>();
        AddChildrenUIComponentsToDic<Image>();
        AddChildrenUIComponentsToDic<Toggle>();
        AddChildrenUIComponentsToDic<Slider>();
        AddChildrenUIComponentsToDic<ScrollRect>();
        AddChildrenUIComponentsToDic<InputField>();
    }                                                                                                                                                        
    protected virtual void OnDestroy()
    {
        ClearDic();
    }
    protected virtual void Update() { }
    
    /// <summary>
    /// ��ť�������鷽��������������д�˷�����Ӿ���Ĵ����߼�
    /// </summary>
    /// <param name="buttonObjName">��ť��������һ������¿����ж����ť��</param>
    /// <param name="button">��ť�������</param>
    protected virtual void OnClick(Button button) { }
    /// <summary>
    /// Toggle �������鷽��
    /// </summary>
    /// <param name="toggleObjName">Toggle ��������һ������¿����ж�� Toggle��</param>
    /// <param name="value">Toggle ֵ</param>
    /// <param name="toggle">Toggle �������</param>
    protected virtual void OnToggleValueChanged(bool value, Toggle toggle) { }
    /// <summary>
    /// Slider �������鷽��
    /// </summary>
    /// <param name="value">Slider ֵ</param>
    /// <param name="slider">Slider �������</param>
    protected virtual void OnSliderValueChanged(float value,Slider slider) { }
    /// <summary>
    /// ��ȡPanel������������T���͵��������ӵ��ֵ��У���ȥ��ӿؼ����û��϶��ؼ��ȷ�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void AddChildrenUIComponentsToDic<T>() where T: UIBehaviour
    {        
        if (uiDic is null)
            return;

        var components = GetComponentsInChildren<T>(true); //��ȡPanel�µ�����T���͵������
        foreach (var uiComponent in components)
        {
            var objName = uiComponent.gameObject.name;
            if (!uiDic.ContainsKey(objName))
                uiDic.Add(objName, new List<UIBehaviour> { uiComponent });
            else
                uiDic[objName].Add(uiComponent);

            switch (uiComponent)
            {
                
                //�������ǰ�ť��ֱ����Ӽ����¼�
                //���������һ����ť��Ϸ����ֻ��Ҫ��һ��PanelBase���ӽű�����дOnClick�������������Զ���������¼�
                case Button buttonComponent:
                    //AddListenerֻ�ܴ��޲�ί�У�������Lambda���ʽ��һ��
                    buttonComponent.onClick.AddListener(() =>
                    {
                        OnClick(buttonComponent);                    
                    });
                    break;
                
                //�������ǵ�ѡ����ѡ��
                case Toggle toggleComponent:
                    toggleComponent.onValueChanged.AddListener((value) =>
                    {
                        OnToggleValueChanged(value, toggleComponent);                       
                    });
                    break;
                case Slider sliderComponent:
                    sliderComponent.onValueChanged.AddListener((value) =>
                    {
                        OnSliderValueChanged(value, sliderComponent);
                    });
                    break;
            }
        }
    }
    /// <summary>
    /// ��ȡ��Ӧ���ֵĶ�Ӧ�ؼ��ű�
    /// PanelBase�����������Start�����е��ô˷�����ȡ��ӦUI�ű�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetUIComponentByName<T>(string name) where T: UIBehaviour
    {
        if (!uiDic.ContainsKey(name))
        {
            return null;
        }
            

        foreach (var uiBehaviour in uiDic[name])
        {
            if (uiBehaviour is T castBehaviour)
                return castBehaviour;
        }
        return null;
    }

    public void ClearDic()
    {
        uiDic?.Clear();
    }
    protected void DestroySelf(Action callback = null)
    {
        UI3DManager.Instance.DestroyPanel(gameObject.name, () =>
        {
            callback?.Invoke();
        });
    }
    protected void HideSelf(Action callback = null)
    {
        UI3DManager.Instance.HidePanel(gameObject.name, () =>
        {
            callback?.Invoke();
        });
    }
}
