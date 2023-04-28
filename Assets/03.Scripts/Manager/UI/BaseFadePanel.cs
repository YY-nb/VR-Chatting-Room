using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// UI���뵭����������
/// </summary>
public class BaseFadePanel : BaseUIPanel
{
    private CanvasGroup canvasGroup;
    [SerializeField]
    protected float alphaShowSpeed = 2;  //�����ٶ�
    [SerializeField]
    protected float alphaHideSpeed = 2;  //�����ٶ�
    protected bool isShow = false;
    private Action hideCallBack;  //���͸����Ϊ0ʱ���õ�ί��
    private Action showCallBack;   //���͸����Ϊ1ʱ���õ�ί��
    protected override void Awake()
    {
        base.Awake(); 
        //һ��ʼ��ȡ����Ϲ��ص����
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); 
        }
    }
    protected override void Update()
    {
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += Time.deltaTime * alphaShowSpeed;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
                SetInteractableOfCanvasGroup(true);
                showCallBack?.Invoke(); 
            }

        }
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaShowSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    protected void SetInteractableOfCanvasGroup(bool isEnbale)
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = isEnbale;
        }
    }
    /// <summary>
    /// ��ʾ�Լ������뵭��ģʽ
    /// </summary>
    /// <param name="callback">UI͸������0�仯��1ʱ���õ�ί��</param>
    public override void Show(Action callback = null)
    {
        isShow = true;
        showCallBack = callback;
        canvasGroup.alpha = 0;
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// �����Լ������뵭��ģʽ
    /// </summary>
    /// <param name="callBack">UI͸��ʱ���õ�ί��</param>
    public override void Hide(Action callBack = null)
    {
        isShow = false;
        hideCallBack = callBack;
        canvasGroup.alpha = 1;
        SetInteractableOfCanvasGroup(false);
    }
}
