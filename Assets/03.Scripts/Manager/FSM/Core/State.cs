using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> : IState<T>
{
    protected State(IStateMachine<T> stateMachine, Enum stateID)
    {
        StateMachine = stateMachine;
        StateId = stateID;
    }
    public Enum StateId { get; protected set; }

    public IStateMachine<T> StateMachine { get; private set; }

    public T Owner => StateMachine.Owner;

    /// <summary>
    /// ��һ��״̬��ת���¼���ӳ��
    /// </summary>
    private Dictionary<Condition, Enum> conditionDic = new Dictionary<Condition, Enum>();

 

    public virtual void Awake()
    {
        
    }

   
    public virtual void OnEnter()
    {
       
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }

    public void AddTransition(Enum toStateID, Condition condition)
    {
        if (conditionDic.ContainsKey(condition))
        {
            Debug.LogWarning($"�������ظ���ӣ�{toStateID}�Ѵ��ڣ�");
        }
        else
        {
            conditionDic.Add(condition, toStateID);
        }
    }

    public Enum CheckTransition()
    {
        foreach(var map in conditionDic)
        {
            //���ת���¼��Ƿ񴥷���������������ת������
            if (map.Key.TransitionCallback.Invoke() == map.Key.TriggerType)
            {
                return map.Value;
            }
        }
        return null;
    }
}
