using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class Condition
{
    /// <summary>
    /// ����״̬ת�����¼�
    /// </summary>
    public Func<bool> TransitionCallback { get; set; }
    /// <summary>
    /// ����Ϊtrue����false����
    /// </summary>
    public bool TriggerType { get; set; }
}
