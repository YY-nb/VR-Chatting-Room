using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTask 
{
    /// <summary>
    /// 延时时间
    /// </summary>
    public float delayedTime;
    /// <summary>
    /// 计时器，计时达到延时时间就执行事件
    /// </summary>
    public float timer;
    /// <summary>
    /// 重复次数，-1为一直重复
    /// </summary>
    public int repeatTimes;
    /// <summary>
    /// 剩余重复次数
    /// </summary>
    public int restRepeatTimes;
    /// <summary>
    /// 事件当前状态：是暂停，还是继续
    /// </summary>
    public bool isPause;
    /// <summary>
    /// 事件
    /// </summary>
    public Action callback;

    public TimerTask(float delayedTime, int repeatTimes, Action callback)
    {
        this.delayedTime = delayedTime;
        timer = 0;
        this.repeatTimes = repeatTimes;
        this.isPause = false;
        this.callback = callback;
        if(repeatTimes == -1)
        {
            restRepeatTimes = -1;
        }
        else
        {
            restRepeatTimes = repeatTimes;
        }
    }

}
