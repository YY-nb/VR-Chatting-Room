using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class PanelManager
{
    protected string canvasTag;
    //把场景中的所有Canvas拖给它
    private GameObject[] canvasArr;

    private List<BasePanel> currentPanels = new List<BasePanel>();

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Dictionary<string, GameObject> canvasDic = new Dictionary<string, GameObject>();

    private Dictionary<string, MethodInfo> ReflectionMethodDic = new Dictionary<string, MethodInfo>();

    public int CurrentPanelsCount => currentPanels.Count;
    public bool IsPanelInList
    {
        get
        {
            return currentPanels.Count > 0;
        }
    }

    public PanelManager()
    {
        InitCanvasTag();
        InitCanvas();
    }
    public abstract void InitCanvasTag();
    /// <summary>
    /// 找到场景中所有WorldSpace模式的Canvas，添加到字典中
    /// WorldSpace模式的Canvas需要添加上 Canvas3D 的 Tag
    /// 进入新的场景后要手动调用此方法进行该场景Canvas3D的初始化管理
    /// </summary>
    public virtual void InitCanvas()
    {
        canvasArr = GameObject.FindGameObjectsWithTag(canvasTag);
        if (canvasArr != null)
        {
            for (int i = 0; i < canvasArr.Length; i++)
            {
                canvasDic.Add(canvasArr[i].name, canvasArr[i]);
#if UNITY_EDITOR
                Debug.Log(canvasArr[i].name);
#endif
            }
        }
    }
    /// <summary>
    /// 动态添加Canvas到字典中
    /// </summary>
    /// <param name="canvas"></param>
    public virtual void AddCanvasToDic(GameObject canvas)
    {
        if (canvas != null)
        {
            canvasDic.Add(canvas.name, canvas);
        }
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="onFinish">面板加载完毕后的委托</param>
    public virtual void ShowPanel<T>(string panelName, string canvasName, Action<T> onFinish = null, Action<T> onBegin = null, bool needSavePanel = true, ResourceLoadWay resourceLoadWay = ResourceLoadWay.Addressables) where T : BasePanel
    {
        if (!canvasDic.ContainsKey(canvasName))
        {
            Debug.LogWarning($"传入的canvas:{canvasName}名字错误,panel:{panelName}");
            return;
        }
        if (panelDic.ContainsKey(panelName))
        {
            if (needSavePanel)
            {
                AddCurrentPanel(panelDic[panelName]);
            }
            panelDic[panelName].Show(() => onFinish?.Invoke(panelDic[panelName] as T),
                ()=> onBegin?.Invoke(panelDic[panelName] as T));
            return;
        }
        string panelPath = null;
        if (resourceLoadWay == ResourceLoadWay.Resources)
        {
            panelPath = ResourceName.UIBasePath + panelName;
        }
        else
        {
            panelPath = panelName;
        }
        //加载Resources文件夹下对应目录的UI预设体，默认路径写在了ResourceName中
        ResourceManager.Instance.LoadAsync<GameObject>(panelPath,
            (obj) =>
            {
                obj.name = panelName;
                //把UI设为对应Canvas的子物体
                obj.transform.SetParent(canvasDic[canvasName].transform, false);
                T panel = obj.GetComponent<T>();
                panelDic.Add(panelName, panel);
                panel.Show(() =>
                {
                    if (needSavePanel)
                    {
                        if (panelDic.ContainsKey(panelName))
                        {
                            AddCurrentPanel(panelDic[panelName]);
                        }                        
                    }
                    onFinish?.Invoke(panel);
                }, () =>
                {
                    onBegin?.Invoke(panel);
                });
            });
    }
    public virtual void ShowLastPanelInList(Action onFinish = null, Action onBegin = null)
    {
        if (currentPanels.Count > 0)
        {
            var panel = currentPanels[currentPanels.Count - 1];
            panel.Show(onFinish, onBegin);
        }
    }

    public virtual void HideLastPanelInList(Action onFinish = null, Action onBegin = null, bool needSavePanel = false)
    {
        if (currentPanels.Count > 0)
        {
            var panel = currentPanels[currentPanels.Count - 1];

            panel.Hide(() =>
            {
                if (!needSavePanel)
                {
                    RemoveCurrentPanel(panel);
                }
                onFinish?.Invoke();
            }, onBegin);
        }
    }
    public virtual void DestroyLastPanelInList(Action callback = null, Action onBegin = null, bool needSavePanel = false)
    {
        if (currentPanels.Count > 0)
        {
            var panel = currentPanels[currentPanels.Count - 1];
            panel.Hide(() =>
            {
                if (!needSavePanel)
                {
                    RemoveCurrentPanel(panel);
                }
                OnDestroyPanel(panel.name);
                callback?.Invoke();
            }, onBegin);
        }
    }
    public void HideAllPanelInList(Action onFinish = null, Action onBegin = null, bool needSavePanel = false)
    {
        if (currentPanels.Count > 0)
        {
            for (int i = currentPanels.Count - 1; i >= 0; i--)
            {
                var panel = currentPanels[i];
                panel.Hide(() =>
                {
                    if (!needSavePanel)
                    {
                        RemoveCurrentPanel(panel);
                    }
                    onFinish?.Invoke();
                }, onBegin);
            }
        }
    }
    public void DestroyAllPanelsInlist(Action onFinish = null, Action onBegin = null, bool needSavePanel = false)
    {
        if (currentPanels.Count > 0)
        {
            for (int i = currentPanels.Count - 1; i >= 0; i--)
            {
                var panel = currentPanels[i];
                panel.Hide(() =>
                {
                    if (!needSavePanel)
                    {
                        RemoveCurrentPanel(panel);
                    }
                    OnDestroyPanel(panel.name);
                    onFinish?.Invoke();
                }, onBegin);
            }
        }
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">面板名</param>
    /// <param name="onFinish">隐藏面板完毕之后的委托</param>
    public virtual void HidePanel(string panelName, Action onFinish = null, Action onBegin = null, bool needSavePanel = false)
    {
        if (panelDic.ContainsKey(panelName))
        {
            if (!needSavePanel)
            {
                RemoveCurrentPanel(panelDic[panelName]);
            }
            panelDic[panelName].Hide(onFinish, onBegin);
        }
    }
    /// <summary>
    /// 销毁面板
    /// </summary>
    /// <param name="panelName">面板名</param>
    /// <param name="onFinish">销毁面板之后的委托</param>
    public virtual void DestroyPanel(string panelName, Action onFinish = null, Action onBegin = null, bool needSavePanel = false)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide(() =>
            {
                if (!needSavePanel)
                {
                    RemoveCurrentPanel(panelDic[panelName]);
                }
                OnDestroyPanel(panelName);
                onFinish?.Invoke();
            }, onBegin);
        }
    }
    /// <summary>
    /// 销毁面板时的回调
    /// </summary>
    /// <param name="panelName"></param>
    private  void OnDestroyPanel(string panelName)
    {
        GameObject.Destroy(panelDic[panelName].gameObject);
        panelDic.Remove(panelName);
    }
    /// <summary>
    /// 得到面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public virtual T GetPanel<T>(string panelName) where T : BasePanel
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
    public virtual void Clear()
    {
        panelDic?.Clear();
        canvasDic?.Clear();
        ReflectionMethodDic?.Clear();
        canvasArr = null;
        currentPanels.Clear();
    }
    private void AddCurrentPanel(BasePanel panel)
    {
        
        for (int i = 0; i < currentPanels.Count; i++)
        {
            if (currentPanels[i].name == panel.name)
            {
                return;
            }
        }
        currentPanels.Add(panel);
    }
    private void RemoveCurrentPanel(BasePanel panel)
    {
        for (int i = currentPanels.Count - 1; i >= 0; i--)
        {
            if (currentPanels[i].name == panel.name)
            {
                currentPanels.RemoveAt(i);
                break;
            }
        }

    }
    private MethodInfo GetMethodByReflection(string methodName, BasePanel panel)
    {
        if (panel == null) return null;
        Type panelType = Type.GetType(panel.name);
        return GetType().GetMethod(methodName).MakeGenericMethod(new Type[] { panelType });
    }
    public virtual void ShowPanelByReflection(object[] paramArr = null)
    {
        for (int i = 0; i < currentPanels.Count; i++)
        {
            BasePanel panel = currentPanels[i];
            string methodName = nameof(ShowPanel);
            string keyName = panel.name + "_" + methodName;
            if (!ReflectionMethodDic.TryGetValue(keyName, out MethodInfo showPanelMethod))
            {
                showPanelMethod = GetMethodByReflection(methodName, panel);
                AddMethodToDic(keyName, showPanelMethod);
            }
            if (paramArr == null)
            {
                showPanelMethod.Invoke(this, new object[] { panel.name, null, null, true });
            }
            else
            {
                showPanelMethod.Invoke(this, paramArr);
            }
        }

    }
    private void AddMethodToDic(string keyName, MethodInfo methodInfo)
    {
        if (!ReflectionMethodDic.ContainsKey(keyName))
        {
            ReflectionMethodDic.Add(keyName, methodInfo);
        }
    }
    private void RemoveMethodFromDic(string keyName)
    {
        if (ReflectionMethodDic.ContainsKey(keyName))
        {
            ReflectionMethodDic.Remove(keyName);
        }
    }
}
