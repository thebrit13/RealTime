using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{
    private DateTime _LastUpdateTime = DateTime.MinValue;
    private const float TASK_UPDATE_INTERVAL = .2f;

    public void Update()
    {
        if((DateTime.Now - _LastUpdateTime).TotalSeconds > TASK_UPDATE_INTERVAL)
        {
            UpdateTasks();
            _LastUpdateTime = DateTime.Now;
        }
    }

    private void UpdateTasks()
    {

    }

    private void RegisterUnit()
    {

    }
}

public class TaskObject
{

}
