using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<T> : IStateMachine<T>
{
    public FiniteStateMachine(T owner)
    {
        Owner = owner;
    }
    public T Owner { get; private set; }

    public Enum DefaultStateID { get ; set ; }

    private Dictionary<Enum, IState<T>> stateDic = new Dictionary<Enum, IState<T>>();

    /// <summary>
    /// 当前状态
    /// </summary>
    private IState<T> currentState;

    /// <summary>
    /// 初始化所有状态
    /// </summary>
    public void Awake()
    {
        foreach(var state in stateDic.Values)
        {
            state.Awake();
        }
    } 

    public void Start()
    {
        if (DefaultStateID == null)
        {
            Debug.LogError("状态机没有设置默认状态");
            return;
        }
        SwitchState(DefaultStateID);
    }

    public void Update()
    {
        Enum newStateID = currentState.CheckTransition(); 
        if (newStateID != null)
        {
            SwitchState(newStateID);
        }
        currentState.OnUpdate();
    }

    public IStateMachine<T> AddState(Enum stateID)
    {
        if (stateDic.ContainsKey(stateID))
        {
            Debug.LogWarning($"尝试重复添加状态{stateID}");
        }
        else
        {
            var state = StateFactory.GetState<T>(this, stateID);
            stateDic.Add(stateID, state);
        }
        return this;
    }

    public IStateMachine<T> AddTransition(Enum fromSateID, Enum toStateID, Condition condition)
    {
        var fromState = GetState(fromSateID);
        var toState = GetState(toStateID);
        if(fromState == null)
        {
            AddState(fromSateID);
            fromState = GetState(fromSateID); 
        }
        if(toState == null)
        {
            AddState(toStateID);
            toState = GetState(toStateID);  
        }
        //往fromState中添加toState和转换事件的映射
        fromState.AddTransition(toStateID, condition);
        return this;
    }

    public void SwitchState(Enum targetStateID)
    {
        currentState?.OnExit();
        //如果传入空值，则退出当前状态
        if(targetStateID == null)
        {
            currentState = null;
            return;
        }
        var targetState = GetState(targetStateID);
        targetState?.OnEnter();
        currentState = targetState; Debug.Log($"switch to {currentState.GetType().Name}");
    }
    public IStateMachine<T> Open(Enum stateID)
    {
        var state = GetState(stateID);
        try
        {
            var compState = state as CompositeState<T>;
            return compState;
        }
        catch (Exception e)
        {
            Debug.LogError($"获取复合状态错误，ID：{stateID}");
            Debug.LogError($"错误类型：{e}");
            throw;
        }
    }
    private IState<T> GetState(Enum stateID)
    {
        if(stateDic.TryGetValue(stateID, out var state))
        {
            return state;
        }
        return null;
    }
}
