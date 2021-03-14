
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 定时任务
/// </summary>
public struct TimingTask
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    private UnityEvent OnFinish;

    public TimingTask(DateTime start,DateTime end)
    {
        this.Start = start;
        this.End = end;
        OnFinish = new UnityEvent();
        TaskManager.Instance.AddTask(this);
    }

    public TimingTask(TimeSpan duration)
    {
        Start = NetworkTime.Instance.CurrentTime;
        End = NetworkTime.Instance.CurrentTime.Add(duration);
        OnFinish = new UnityEvent();
        TaskManager.Instance.AddTask(this);
    }

    public void AddListener(UnityAction action)
    {
        OnFinish.AddListener(action);
    }

    public void Finish()
    {
        OnFinish.Invoke();
    }
}
