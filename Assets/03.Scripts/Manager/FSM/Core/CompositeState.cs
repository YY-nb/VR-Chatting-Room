using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeState<T> : State<T>, IStateMachine<T>
{
    protected CompositeState(IStateMachine<T> stateMachine, Enum stateID) : base(stateMachine, stateID)
    {
        innerStateMachine = new FiniteStateMachine<T>(Owner);
    }

    private readonly FiniteStateMachine<T> innerStateMachine;

    public Enum DefaultStateID
    {
        get => innerStateMachine.DefaultStateID;
        set => innerStateMachine.DefaultStateID = value;
    }
    #region 状态抽象类的方法
    public override void Awake()
    {
        base.Awake();
        innerStateMachine.Awake();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        innerStateMachine.Start();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        innerStateMachine.Update();
    }

    public override void OnExit()
    {
        base.OnExit();
        innerStateMachine.SwitchState(null);
    }
    #endregion

    #region 状态机接口的方法
    public IStateMachine<T> AddState(Enum stateID)
    {
        innerStateMachine.AddState(stateID);
        return this;
    }

    public IStateMachine<T> AddTransition(Enum fromSateID, Enum toStateID, Condition condition)
    {
        innerStateMachine.AddTransition(fromSateID, toStateID, condition);
        return this;
    }

    public void SwitchState(Enum targetStateID)
    {
        innerStateMachine.SwitchState(targetStateID);
    }

    public IStateMachine<T> Open(Enum stateID)
    {
        return innerStateMachine.Open(stateID);
    }

    #endregion
}
