using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    MOVE,
    WAIT,
    WORK
}

public class TaskManager
{
    private DateTime _LastUpdateTime = DateTime.MinValue;
    private const float TASK_UPDATE_INTERVAL = .2f;

    private TaskObject _CurrentTask;

    private Queue<TaskObject> _TaskQueue = new Queue<TaskObject>();

    private System.Action<Vector3> _MoveToCallback;

    private const float STOPPING_DISTANCE_FOR_MOVE = .5f;

    private BaseUnit _BaseUnitRef;

    public void Setup(BaseUnit unitRef,System.Action<Vector3> moveCallback)
    {
        _BaseUnitRef = unitRef;
        _MoveToCallback = moveCallback;
    }

    public void AddTask(float duration, bool addTask = false)
    {
        TaskObject_Wait to = new TaskObject_Wait()
        {
            CurrentTaskType = TaskType.WAIT,
            Duration = duration
        };
        AddTaskInternal(to,addTask);
    }

    public void AddTask(Vector3 destination, bool addTask = false)
    {
        TaskObject_Move to = new TaskObject_Move()
        {
            CurrentTaskType = TaskType.MOVE,
            Destination = destination
        };
        AddTaskInternal(to,addTask);
    }

    public void AddTask(BaseWorkable workable, bool addTask = false)
    {
        TaskObject_Work to = new TaskObject_Work();
        AddTaskInternal(to,addTask);
    }

    private void AddTaskInternal(TaskObject to,bool addTask)
    {
        if(!addTask || !Input.GetKey(KeyCode.LeftShift))
        {
            _TaskQueue.Clear();
            _CurrentTask = null;
        }
        _TaskQueue.Enqueue(to);
    }

    public void UpdateTaskManager()
    {
        if(_CurrentTask != null)    
        {
            if((DateTime.Now - _LastUpdateTime).TotalSeconds > TASK_UPDATE_INTERVAL)
            {
                UpdateTask();
                _LastUpdateTime = DateTime.Now;
            }
        }
        else
        {
            if(_TaskQueue.Count > 0)
            {
                _CurrentTask = _TaskQueue.Dequeue();
                SetupTask();
            }
        }
    }

    private void SetupTask()
    {
        switch (_CurrentTask.CurrentTaskType)
        {
            case TaskType.MOVE:
                _MoveToCallback?.Invoke(((TaskObject_Move)_CurrentTask).Destination);
                break;
            case TaskType.WAIT:
                //Double cast, not great
                ((TaskObject_Wait)_CurrentTask).EndTime = DateTime.Now.AddSeconds(((TaskObject_Wait)_CurrentTask).Duration);
                break;
            case TaskType.WORK:
                break;
        }
    }

    private void UpdateTask()
    {
        bool taskDone = false;
        switch(_CurrentTask.CurrentTaskType)
        {
            case TaskType.MOVE:
                if(Vector3.Distance(((TaskObject_Move)_CurrentTask).Destination,_BaseUnitRef.transform.position) < STOPPING_DISTANCE_FOR_MOVE)
                {
                    taskDone = true;
                }
                break;
            case TaskType.WAIT:
                if(DateTime.Now >= ((TaskObject_Wait)_CurrentTask).EndTime)
                {
                    taskDone = true;
                }
                break;
            case TaskType.WORK:
                break;
        }

        if(taskDone)
        {
            _CurrentTask.FinishCallback?.Invoke();
            _CurrentTask = null;
        }
    }
}

public class TaskObject
{
    public TaskType CurrentTaskType;
    public System.Action FinishCallback;
}

public class TaskObject_Wait : TaskObject
{
    public float Duration;
    public DateTime EndTime;
}

public class TaskObject_Move:TaskObject
{
    public Vector3 Destination;
}

public class TaskObject_Work:TaskObject
{
    public BaseWorkable Workable;
}
