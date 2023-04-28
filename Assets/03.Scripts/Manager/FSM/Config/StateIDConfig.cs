using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player_State
{
    /// <summary>
    /// �����ϣ�
    /// </summary>
    Alive,
    /// <summary>
    /// ����
    /// </summary>
    Dead,
    /// <summary>
    /// ִ��ָ����ϣ�
    /// </summary>
    Executing,
    /// <summary>
    /// ��ֹ
    /// </summary>
    Idle,
    /// <summary>
    /// �ƶ������ϣ�
    /// </summary>
    Move,
    /// <summary>
    /// �ƶ���״̬��Ĭ��״̬
    /// </summary>
    MoveDefault,
    /// <summary>
    /// ��·
    /// </summary>
    Walk,
    /// <summary>
    /// �ܲ�
    /// </summary>
    Run,
    /// <summary>
    /// �����������ϣ�
    /// </summary>
    Jumping,
    /// <summary>
    /// ������
    /// </summary>
    JumpDown,
    /// <summary>
    /// ��ǰ��
    /// </summary>
    JumpForward,
    /// <summary>
    /// ������
    /// </summary>
    JumpUp,
    /// <summary>
    /// ��Ծ��״̬��Ĭ��״̬
    /// </summary>
    JumpDefault,
    /// <summary>
    /// Я����Ʒ
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

