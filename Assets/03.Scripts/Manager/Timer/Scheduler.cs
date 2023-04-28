using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : SingletonAutoMonoBase<Scheduler>
{
    private SingleLinkedList<TimerTask> timers =new SingleLinkedList<TimerTask>();
    private TimerTask tempTimer;
    private void Update()
    {
        UpdateTimers();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Clear();
    }
    private void UpdateTimers()
    {
        if (timers.Count == 0) return;
        for(int i=0; i < timers.Count; i++)
        {
            tempTimer = timers[i];
            if (!tempTimer.isPause)
            {
                tempTimer.timer += Time.deltaTime; 
            }
            //达到延迟时间，触发事件
            if (tempTimer.timer >= tempTimer.delayedTime)
            {
                //print("定时事件触发");
                tempTimer.callback?.Invoke();
                if (tempTimer.restRepeatTimes == 0)
                {
                    timers.DeleteAt(i);
                    --i;
                    if (timers.Count == 0) break;
                }
                else
                {
                    if (tempTimer.repeatTimes > -1)
                    {
                        tempTimer.restRepeatTimes--;
                    }
                    tempTimer.timer = 0;
                }
            }
            
        }
        
    }
    public void AddTimerTask(float delayedTime, Action callback, int repeatTimes = -1)
    {
        if(callback != null)
        {
            bool isContain = false;
            for (int i = 0; i < timers.Count; i++)
            {
                if (timers[i].callback.Equals(callback))
                {
                    isContain = true;
                    break;
                }
            }
            if (!isContain)
            {
                timers.AddLast(new TimerTask(delayedTime, repeatTimes, callback));
            }
        }
        
    }
    /// <summary>
    /// 暂停延时事件的计时
    /// </summary>
    /// <param name="callback"></param>
    public void Pause(Action callback)
    {
        if (callback != null)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                var taskTemp = timers[i];
                if (taskTemp.callback.Equals(callback))
                {
                    taskTemp.isPause = true;
                }
            }
        }

    }

    /// <summary>
    /// 结束事件的计时暂停状态
    /// </summary>
    /// <param name="callback"></param>
    public void UnPause(Action callback)
    {
        if (callback != null)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                var taskTemp = timers[i];
                if (taskTemp.callback.Equals(callback))
                {
                    taskTemp.isPause = false;
                }
            }
        }
    }

    /// <summary>
    /// 移除指定事件
    /// </summary>
    /// <param name="callback"></param>
    public void Remove(Action callback)
    {
        if (callback != null)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                if (timers[i].callback.Equals(callback))
                {
                    timers.DeleteAt(i);
                }
            }
        }
    }
    /// <summary>
    /// 清空定时任务
    /// </summary>
    public void Clear()
    {
        timers.Clear();
    }


}
