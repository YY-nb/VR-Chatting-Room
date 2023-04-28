using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 状态类接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IState<T>
{
    Enum StateId { get; }

    /// <summary>
    /// 状态的拥有者
    /// </summary>
    T Owner { get; }

    /// <summary>
    /// 状态所属的状态机
    /// </summary>
    IStateMachine<T> StateMachine { get; }

    /// <summary>
    /// 状态初始化
    /// </summary>
    void Awake();

    /// <summary>
    /// 进入状态
    /// </summary>
    void OnEnter();

    /// <summary>
    /// 执行状态
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// 退出状态
    /// </summary>
    void OnExit();

    /// <summary>
    ///  添加状态转换条件
    /// </summary>
    /// <param name="toStateID"></param>
    /// <param name="condition"></param>
    void AddTransition(Enum toStateID, Condition condition);

    /// <summary>
    /// 转换条件检查
    /// </summary>
    /// <returns></returns>
    Enum CheckTransition();
    
}
