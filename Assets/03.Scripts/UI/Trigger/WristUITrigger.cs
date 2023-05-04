using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristUITrigger : MonoBehaviour
{
    private Transform head;
    private Vector3 wristVector;
    private Vector3 headVector;
    private bool isEnable;
    private bool isShowing;
    private bool isHiding;
    private bool canHide;
    [SerializeField] private float openAngle = 50f;


    private void Awake()
    {
        EventCenter.Instance.AddListener<bool>(EventName.OnEnableWristUI, EnableWristUI);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveListener<bool>(EventName.OnEnableWristUI, EnableWristUI);
    }


    void Start()
    {
        head = Camera.main.transform;
        isEnable = true;
    }


    void Update()
    {
        if (isEnable)
        {
            CheckRayOnWristUI();
        }
    }

    private void CheckRayOnWristUI()
    {
        if (!isShowing)
        {
            float thisAngle = GetJudgeAngle(); //print($"angle:{thisAngle}");
            if (thisAngle < openAngle && !isShowing && IsInputTriggered())
            {
                isShowing = true;
                //show wrist UI
                UI3DManager.Instance.ShowPanel<WristUIPanel>(nameof(WristUIPanel), CanvasName.WristUICanvas, (panel) =>
                {
                    canHide = true;
                });
            }
        }
        else if(canHide)
        {
            if (!isHiding)
            {
                float thisAngle = GetJudgeAngle(); //print($"angle:{thisAngle}");
                if (thisAngle >= openAngle)
                {
                    isHiding = true;
                    UI3DManager.Instance.HidePanel(nameof(WristUIPanel), () =>
                    {
                        isHiding = false;
                        isShowing = false;
                        canHide = false;
                    });
                }
            }
        }


    }
    private float GetJudgeAngle()
    {
        wristVector = transform.forward;
        headVector = head.position - transform.position;
        return Vector3.Angle(wristVector, headVector);
    }
    private bool IsInputTriggered()
    {
        return InputBridge.Instance.GetControllerBindingValue(ControllerBinding.LeftGrip) && InputBridge.Instance.GetControllerBindingValue(ControllerBinding.LeftTrigger);
    }
    private void EnableWristUI(bool isEnable)
    {
        this.isEnable = isEnable;
        if (!isEnable)
        {
            isShowing = false;
            isHiding = false;
        }
    }
}
