using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class PanelManager
{
    protected string canvasTag;
    //�ѳ����е�����Canvas�ϸ���
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
    /// �ҵ�����������WorldSpaceģʽ��Canvas����ӵ��ֵ���
    /// WorldSpaceģʽ��Canvas��Ҫ����� Canvas3D �� Tag
    /// �����µĳ�����Ҫ�ֶ����ô˷������иó���Canvas3D�ĳ�ʼ������
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
    /// ��̬���Canvas���ֵ���
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
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">���ű���</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="callback">��������Ϻ��ί��</param>
    public virtual void ShowPanel<T>(string panelName, string canvasName, Action<T> callback = null, bool needSavePanel = true, ResourceLoadWay resourceLoadWay = ResourceLoadWay.Addressables) where T : BasePanel
    {
        if (!canvasDic.ContainsKey(canvasName))
        {
            Debug.LogWarning($"�����canvas:{canvasName}���ִ���,panel:{panelName}");
            return;
        }
        if (panelDic.ContainsKey(panelName))
        {
            if (needSavePanel)
            {
                AddCurrentPanel(panelDic[panelName]);
            }
            panelDic[panelName].Show(() => callback?.Invoke(panelDic[panelName] as T));
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
        //����Resources�ļ����¶�ӦĿ¼��UIԤ���壬Ĭ��·��д����ResourceName��
        ResourceManager.Instance.LoadAsync<GameObject>(panelPath,
            (obj) =>
            {
                obj.name = panelName;
                //��UI��Ϊ��ӦCanvas��������
                obj.transform.SetParent(canvasDic[canvasName].transform, false);
                T panel = obj.GetComponent<T>();
                panelDic.Add(panelName, panel);
                panel.Show(() =>
                {
                    if (needSavePanel)
                    {
                        AddCurrentPanel(panelDic[panelName]);
                    }
                    callback?.Invoke(panel);
                });
            });
    }
    public virtual void ShowLastPanelInList(Action callback = null)
    {
        if (currentPanels.Count > 0)
        {
            var panel = currentPanels[currentPanels.Count - 1];
            panel.Show(() =>
            {
                callback?.Invoke();
            });
        }
    }
    public virtual void HideLastPanelInList(Action callback = null, bool needSavePanel = false)
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
                callback?.Invoke();
            });
        }
    }
    public virtual void DestroyLastPanelInList(Action callback = null, bool needSavePanel = false)
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
            });
        }
    }
    public void HideAllPanelInList(Action callback = null, bool needSavePanel = false)
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
                    callback?.Invoke();
                });
            }
        }
    }
    public void DestroyAllPanelsInlist(Action callback = null, bool needSavePanel = false)
    {
        if(currentPanels.Count > 0)
        {
            for(int i = currentPanels.Count - 1; i >= 0; i--)
            {
                var panel = currentPanels[i];
                panel.Hide(() =>
                {
                    if (!needSavePanel)
                    {
                        RemoveCurrentPanel(panel);
                    }
                    OnDestroyPanel(panel.name);
                    callback?.Invoke();
                });
            }
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">�����</param>
    /// <param name="callback">����������֮���ί��</param>
    public virtual void HidePanel(string panelName, Action callback = null, bool needSavePanel = false)
    {
        if (panelDic.ContainsKey(panelName))
        {
            if (!needSavePanel)
            {
                RemoveCurrentPanel(panelDic[panelName]);
            }
            panelDic[panelName].Hide(callback);
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">�����</param>
    /// <param name="callback">�������֮���ί��</param>
    public virtual void DestroyPanel(string panelName, Action callback = null, bool needSavePanel = false)
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
                callback?.Invoke();
            });
        }
    }
    /// <summary>
    /// �������ʱ�Ļص�
    /// </summary>
    /// <param name="panelName"></param>
    private  void OnDestroyPanel(string panelName)
    {
        GameObject.Destroy(panelDic[panelName].gameObject);
        panelDic.Remove(panelName);
    }
    /// <summary>
    /// �õ����
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
    /// ����ֵ�
    /// </summary>
    public virtual void ClearDic()
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
