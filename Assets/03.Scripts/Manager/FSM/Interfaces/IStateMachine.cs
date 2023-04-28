using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<T>
{
    /// <summary>
    /// ״̬����
    /// </summary>
    T Owner { get; }

    /// <summary>
    /// Ĭ��״̬
    /// </summary>
    Enum DefaultStateID { get; set; }

    /// <summary>
    /// ���״̬
    /// </summary>
    /// <param name="state"></param>
    IStateMachine<T> AddState(Enum stateID);

    /// <summary>
    /// �л�״̬
    /// </summary>
    /// <param name="targetState"></param>
    void SwitchState(Enum targetStateID);

    /// <summary>
    /// ����ת��״̬
    /// </summary>
    /// <param name="fromSateID"></param>
    /// <param name="toStateID"></param>
    /// <param name="transitionCallback"></param>
    /// <returns></returns>
    IStateMachine<T> AddTransition(Enum fromSateID, Enum toStateID, Condition condition);

    /// <summary>
    /// �򿪸���״̬����״̬��
    /// </summary>
    /// <param name="stateID">����״̬ID</param>
    /// <returns>��״̬��</returns>
    IStateMachine<T> Open(Enum stateID);
}
