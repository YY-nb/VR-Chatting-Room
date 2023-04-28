using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ״̬��ӿ�
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IState<T>
{
    Enum StateId { get; }

    /// <summary>
    /// ״̬��ӵ����
    /// </summary>
    T Owner { get; }

    /// <summary>
    /// ״̬������״̬��
    /// </summary>
    IStateMachine<T> StateMachine { get; }

    /// <summary>
    /// ״̬��ʼ��
    /// </summary>
    void Awake();

    /// <summary>
    /// ����״̬
    /// </summary>
    void OnEnter();

    /// <summary>
    /// ִ��״̬
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// �˳�״̬
    /// </summary>
    void OnExit();

    /// <summary>
    ///  ���״̬ת������
    /// </summary>
    /// <param name="toStateID"></param>
    /// <param name="condition"></param>
    void AddTransition(Enum toStateID, Condition condition);

    /// <summary>
    /// ת���������
    /// </summary>
    /// <returns></returns>
    Enum CheckTransition();
    
}
