
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务管理器,用于管理定时任务
/// </summary>
public class TaskManager : MonoSingleton<TaskManager>
{
    /// <summary>
    /// 任务列表
    /// </summary>
    private List<TimingTask> tasks = new List<TimingTask>();
    List<TimingTask> removeList = new List<TimingTask>();

    private void Start()
    {
        StartCoroutine(CheckTask());
    }
    
    /// <summary>
    /// 添加一个任务
    /// </summary>
    public void AddTask(TimingTask task)
    {
        tasks.Add(task);
    }
    
    /// <summary>
    /// 取行一个任务
    /// </summary>
    /// <param name="task"></param>
    public void RemoveTask(TimingTask task)
    {
        removeList.Add(task);
    }
    
    /// <summary>
    /// 判断任务是否到时完成
    /// </summary>
    IEnumerator CheckTask()
    {

        while (true)
        {
            foreach (var item in tasks)
            {
                if (item.End.ToFileTime() - NetworkTime.Instance.CurrentTime.ToFileTime()< 0)
                {
                    item.Finish();
                    removeList.Add(item);
                }
            }

            foreach (var item in removeList)
            {
                tasks.Remove(item);
            }
            removeList.Clear();
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
