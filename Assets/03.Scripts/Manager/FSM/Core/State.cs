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
    /// 下一个状态与转换事件的映射
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
            Debug.LogWarning($"条件被重复添加，{toStateID}已存在！");
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
            //检测转换事件是否触发，若触发则满足转换条件
            if (map.Key.TransitionCallback.Invoke() == map.Key.TriggerType)
            {
                return map.Value;
            }
        }
        return null;
    }
}
