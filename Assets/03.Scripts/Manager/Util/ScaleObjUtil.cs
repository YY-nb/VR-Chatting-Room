using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class ScaleObjUtil
{ 
    public static void ScaleX(Transform obj,float scaleX, float waitTime, Action callback = null)
    {
        if (callback != null)
        {
            MonoManager.Instance.StartCoroutine(I_Callback(callback, waitTime));
        }
        
        obj.DOScaleX(scaleX, waitTime);
    }
  
    public static void ScaleY(Transform obj, float scaleY, float waitTime, Action callback = null)
    {
        if (callback != null)
        {
            MonoManager.Instance.StartCoroutine(I_Callback(callback, waitTime));
        }

        obj.DOScaleY(scaleY, waitTime);
    }
    public static void Scale(Transform obj, Vector3 scale, float waitTime, Action callback = null)
    {
        if (callback != null)
        {
            MonoManager.Instance.StartCoroutine(I_Callback(callback, waitTime));
        }

        obj.DOScale(scale, waitTime);
    }
    private static IEnumerator I_Callback(Action callback, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }

    public static void ScaleUISizeDelta(RectTransform rectTransform, Vector2 size, float waitTime, Action callback = null)
    {
        if (callback != null)
        {
            MonoManager.Instance.StartCoroutine(I_Callback(callback, waitTime));
        }
        rectTransform.DOSizeDelta(size, waitTime);
    }

    public static void ScaleUIWidth(RectTransform rectTransform, float toWidth, float speed, Action callback = null)
    {
        MonoManager.Instance.StartCoroutine(I_ScaleUIWidth(rectTransform, toWidth, speed, callback));
    }
    private static IEnumerator I_ScaleUIWidth(RectTransform rectTransform, float toWidth, float speed, Action callback = null)
    {
        while(rectTransform.rect.width < toWidth)
        {
            float deltaWidth = Time.deltaTime * speed;
            Vector2 newSize;
            if(rectTransform.rect.width + deltaWidth > toWidth)
            {
                newSize = new Vector2(toWidth, rectTransform.rect.height);
                SetRectTransformSize(rectTransform, newSize);
                callback?.Invoke();
            }
            newSize = new Vector2(rectTransform.rect.width + deltaWidth, rectTransform.rect.height);
            SetRectTransformSize(rectTransform, newSize);
            yield return null;
        }
       
        

    }
    public static void SetRectTransformSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
}
