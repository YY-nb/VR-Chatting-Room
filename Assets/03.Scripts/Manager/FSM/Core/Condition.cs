using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 条件类
/// </summary>
public class Condition
{
    /// <summary>
    /// 触发状态转换的事件
    /// </summary>
    public Func<bool> TransitionCallback { get; set; }
    /// <summary>
    /// 条件为true还是false触发
    /// </summary>
    public bool TriggerType { get; set; }
}
