using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    NONE,
    MOVE,
    WAIT,
    WORK,
    DEPOSIT,
    LOOP
}

public class TaskManager
{
    private DateTime _LastUpdateTime = DateTime.MinValue;
    private const float TASK_UPDATE_INTERVAL = .2f;

    private TaskObject _CurrentTask;

    private Queue<TaskObject> _TaskQueue = new Queue<TaskObject>();

    private System.Action<Vector3> _MoveToCallback;

    private const float STOPPING_DISTANCE_FOR_MOVE = .5f;
    private const float STOPPING_DISTANCE_FOR_WORK = 2.0f;

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
        TaskObject_Work to = new TaskObject_Work()
        {
            CurrentTaskType = TaskType.WORK,
            Workable = workable
        };

        TaskObject_Wait to2 = new TaskObject_Wait()
        {
            CurrentTaskType = TaskType.WAIT,
            Duration = 2.0f
        };

        TaskObject_Move to3 = new TaskObject_Move()
        {
            CurrentTaskType = TaskType.MOVE,
            Destination = Vector3.zero
        };

        List<TaskObject> loopTasks = new List<TaskObject>();
        loopTasks.Add(to);
        loopTasks.Add(to2);
        loopTasks.Add(to3);

        TaskObject_Loop toLoop = new TaskObject_Loop()
        {
            CurrentTaskType = TaskType.LOOP,
            TaskObjectLoop = loopTasks
        };

        AddTaskInternal(toLoop,addTask);
    }

    //Need some sort of way to deposit 
    //public void AddTask(BaseWorkable workable, bool addTask = false)
    //{
    //    TaskObject_Work to = new TaskObject_Work()
    //    {
    //        Workable = workable
    //    };
    //    AddTaskInternal(to, addTask);
    //}

    private void AddTaskInternal(TaskObject to,bool addTask)
    {
        if(!addTask) //|| !Input.GetKey(KeyCode.LeftShift))
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
        //probably want to make this a function
        TaskObject taskObjectOverride = null;
        CheckForLoopOverride(ref taskObjectOverride);

        SwitchSetup(taskObjectOverride != null ? taskObjectOverride : _CurrentTask);
    }

    private void UpdateTask()
    {
        TaskObject taskObjectOverride = null;
        CheckForLoopOverride(ref taskObjectOverride);

        bool taskDone = SwitchUpdate(taskObjectOverride != null ? taskObjectOverride : _CurrentTask);

        if(taskDone)
        {
            if(_CurrentTask.CurrentTaskType == TaskType.LOOP)
            {
                ((TaskObject_Loop)_CurrentTask).UpdateIndex();
                SetupTask();
            }
            else
            {
                _CurrentTask.FinishCallback?.Invoke();
                _CurrentTask = null;
            }

        }
    }

    private void CheckForLoopOverride(ref TaskObject taskObjectOverride)
    {
        if (_CurrentTask.CurrentTaskType == TaskType.LOOP)
        {
            //Double cast, not great
            taskObjectOverride = ((TaskObject_Loop)_CurrentTask).TaskObjectLoop[((TaskObject_Loop)_CurrentTask).TaskIndex];
        }
    }

    private bool SwitchUpdate(TaskObject taskObject)
    {
        bool taskDone = false;
        switch (taskObject.CurrentTaskType)
        {
            case TaskType.MOVE:
                if (Vector3.Distance(((TaskObject_Move)taskObject).Destination, _BaseUnitRef.transform.position) < STOPPING_DISTANCE_FOR_MOVE)
                {
                    taskDone = true;
                }
                break;
            case TaskType.WAIT:
                if (DateTime.Now >= ((TaskObject_Wait)taskObject).EndTime)
                {
                    taskDone = true;
                }
                break;
            case TaskType.WORK:
                if (Vector3.Distance(((TaskObject_Work)taskObject).Workable.transform.position, _BaseUnitRef.transform.position) < STOPPING_DISTANCE_FOR_WORK)
                {
                    taskDone = true;
                }
                break;
        }
        return taskDone;
    }

    private void SwitchSetup(TaskObject taskObject)
    {
        switch (taskObject.CurrentTaskType)
        {
            case TaskType.MOVE:
                _MoveToCallback?.Invoke(((TaskObject_Move)taskObject).Destination);
                break;
            case TaskType.WAIT:
                //Double cast, not great
                ((TaskObject_Wait)taskObject).EndTime = DateTime.Now.AddSeconds(((TaskObject_Wait)taskObject).Duration);
                break;
            case TaskType.WORK:
                _MoveToCallback?.Invoke(((TaskObject_Work)taskObject).Workable.transform.position);
                break;
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

public class TaskObject_Loop : TaskObject
{
    public int TaskIndex = 0;
    public List<TaskObject> TaskObjectLoop;

    public void UpdateIndex()
    {
        ++TaskIndex;
        if(TaskIndex >= TaskObjectLoop.Count)
        {
            TaskIndex = 0;
        }
    }
}
