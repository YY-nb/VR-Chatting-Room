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
    /// ��ǰ״̬
    /// </summary>
    private IState<T> currentState;

    /// <summary>
    /// ��ʼ������״̬
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
            Debug.LogError("״̬��û������Ĭ��״̬");
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
            Debug.LogWarning($"�����ظ����״̬{stateID}");
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
        //��fromState�����toState��ת���¼���ӳ��
        fromState.AddTransition(toStateID, condition);
        return this;
    }

    public void SwitchState(Enum targetStateID)
    {
        currentState?.OnExit();
        //��������ֵ�����˳���ǰ״̬
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
            Debug.LogError($"��ȡ����״̬����ID��{stateID}");
            Debug.LogError($"�������ͣ�{e}");
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
