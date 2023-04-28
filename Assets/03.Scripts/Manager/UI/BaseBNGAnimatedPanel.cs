using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBNGAnimatedPanel : BaseAnimatedPanel
{
    [SerializeField] private float vibrateFrequency = 0.1f;
    [SerializeField] private float vibrateValue = 0.1f;
    [SerializeField] private float vibrateDuration = 0.1f;
    private InputBridge input;
    protected override void Awake()
    {
        base.Awake();
        input = InputBridge.Instance;
    }
    protected override void OnClick(UnityEngine.UI.Button button)
    {
        base.OnClick(button);
        
    }
}
