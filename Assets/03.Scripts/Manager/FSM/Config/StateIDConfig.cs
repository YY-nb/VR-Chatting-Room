using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player_State
{
    /// <summary>
    /// 存活（复合）
    /// </summary>
    Alive,
    /// <summary>
    /// 死亡
    /// </summary>
    Dead,
    /// <summary>
    /// 执行指令（复合）
    /// </summary>
    Executing,
    /// <summary>
    /// 静止
    /// </summary>
    Idle,
    /// <summary>
    /// 移动（复合）
    /// </summary>
    Move,
    /// <summary>
    /// 移动子状态的默认状态
    /// </summary>
    MoveDefault,
    /// <summary>
    /// 走路
    /// </summary>
    Walk,
    /// <summary>
    /// 跑步
    /// </summary>
    Run,
    /// <summary>
    /// 向上跳（复合）
    /// </summary>
    Jumping,
    /// <summary>
    /// 向下跳
    /// </summary>
    JumpDown,
    /// <summary>
    /// 向前跳
    /// </summary>
    JumpForward,
    /// <summary>
    /// 向上跳
    /// </summary>
    JumpUp,
    /// <summary>
    /// 跳跃子状态的默认状态
    /// </summary>
    JumpDefault,
    /// <summary>
    /// 携带物品
    /// </summary>
    Carrying,
}

public enum Player_PlatformJumpState
{
    JumpUp,
    JumpDown,
    None
}

public enum FlyRobot_State
{
    Flying,
    Alerting
}

public enum SpiderRobot_State 
{ 
    Alive,
    Idle,
    Attack,
    Dead
}

