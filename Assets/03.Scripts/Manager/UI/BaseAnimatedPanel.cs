using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAnimatedPanel : BaseUIPanel
{
    [SerializeField] protected float showTime = 0.2f;
    [SerializeField] protected float hideTime = 0.2f;

    protected Vector3 originScale;
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        MusicManager.Instance.StartSound("PokeAudio");
    }
    protected override void Awake()
    {
        base.Awake();
        originScale = transform.localScale;
        transform.localScale = new Vector3(0, originScale.y, originScale.z);
        //transform.localScale = Vector3.zero;

    }
    public override void Show(Action onFinish = null, Action onBegin = null)
    {
        onBegin?.Invoke();
        ScaleObjUtil.ScaleX(transform, originScale.x, showTime, onFinish); 
        // ScaleObjUtil.Scale(transform, originScale, showTime, callback);

    }
    public override void Hide(Action callback = null, Action onBegin = null)
    {
        onBegin?.Invoke();
         ScaleObjUtil.ScaleX(transform, 0, hideTime, callback);
        // ScaleObjUtil.Scale(transform, Vector3.zero, hideTime, callback);

    }

}
