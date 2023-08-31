using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum TaskType
{
    NONE,
    MOVE,
    WAIT,
    WORK,
    ATTACK,
    LIST
}

public class TaskManager
{
    private DateTime _LastTaskUpdateTime = DateTime.MinValue;
    private DateTime _LastAIUpdateTime = DateTime.MinValue;
    private const float TASK_UPDATE_INTERVAL = .2f;
    private const float AI_UPDATE_INTERVAL = .2f;

    private TaskObject _CurrentTask;

    private Queue<TaskObject> _TaskQueue = new Queue<TaskObject>();

    private System.Action<Vector3> _MoveToCallback;

    private const float STOPPING_DISTANCE_FOR_MOVE = 1.0f;
    private const float STOPPING_DISTANCE_FOR_WORK = 2.0f;
    private const float STOPPING_DISTANCE_FOR_DROPOFF = 4.0f;

    private BaseUnit _BaseUnitRef;

    private bool _IsAI;

    public void Setup(BaseUnit unitRef,bool isAI,System.Action<Vector3> moveCallback)
    {
        _BaseUnitRef = unitRef;
        _MoveToCallback = moveCallback;
        _IsAI = isAI;
    }

    public void ClearAllTasks()
    {
        _TaskQueue.Clear();
        _CurrentTask = null;
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

    public void AddTask(Vector3 destination,Transform followObject, bool addTask = false)
    {
        TaskObject_Move to = new TaskObject_Move()
        {
            CurrentTaskType = TaskType.MOVE,
            FollowObject = followObject,
            StoppingDistance = STOPPING_DISTANCE_FOR_MOVE,
            Destination = destination
        };
        AddTaskInternal(to,addTask);
    }

    public void AddTask(BaseUnit enemyUnit,float timeBetweenAttacks,int damageAmt)
    {
        TaskObject_Attack to = new TaskObject_Attack()
        {
            CurrentTaskType = TaskType.ATTACK,
            Enemy = enemyUnit,
            TimeBetweenAttacks = timeBetweenAttacks,
            DamageAmt = damageAmt
        };

        //List<TaskObject> loopTasks = new List<TaskObject>();
        //loopTasks.Add(to);

        //TaskObject_List toLoop = new TaskObject_List()
        //{
        //    CurrentTaskType = TaskType.LIST,
        //    TaskObjectLoop = loopTasks,
        //    ShouldLoop = true
        //};

        AddTaskInternal(to, false);
    }

    public void AddTask(BaseWorkable workable,Vector3 dropOffLoc,System.Action workStopCallback,System.Action dropOffCallback,bool addTask = false)
    {
        TaskObject_Move to = new TaskObject_Move()
        {
            CurrentTaskType = TaskType.MOVE,
            Destination = workable.transform.position,
            StoppingDistance = STOPPING_DISTANCE_FOR_WORK
        };

        TaskObject_Work to2 = new TaskObject_Work()
        {
            CurrentTaskType = TaskType.WORK,
            Workable = workable,
            FinishCallback = workStopCallback
        };

        TaskObject_Move to3 = new TaskObject_Move()
        {
            CurrentTaskType = TaskType.MOVE,
            StoppingDistance = STOPPING_DISTANCE_FOR_DROPOFF,
            Destination = dropOffLoc,
            FinishCallback = dropOffCallback
        };

        List<TaskObject> loopTasks = new List<TaskObject>();
        loopTasks.Add(to);
        loopTasks.Add(to2);
        loopTasks.Add(to3);

        TaskObject_List toLoop = new TaskObject_List()
        {
            CurrentTaskType = TaskType.LIST,
            TaskObjectLoop = loopTasks,
            ShouldLoop = true
        };

        AddTaskInternal(toLoop,addTask);
    }

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
        if (_IsAI && (DateTime.Now - _LastAIUpdateTime).TotalSeconds > AI_UPDATE_INTERVAL)
        {
            UpdateAI();
            _LastAIUpdateTime = DateTime.Now;
        }

        if (_CurrentTask != null)    
        {
            if((DateTime.Now - _LastTaskUpdateTime).TotalSeconds > TASK_UPDATE_INTERVAL)
            {
                UpdateTask();
                _LastTaskUpdateTime = DateTime.Now;
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

    private void UpdateAI()
    {
        if(_CurrentTask != null)
        {
            return;
        }
        //check for visual to attack
        //attack (should be handled automatically)
        //move for x amount of seconds
        //Will probably want to have transforms set this max min
        float xLoc = UnityEngine.Random.Range(-50.0f, 50.0f);
        float yLoc = UnityEngine.Random.Range(-50.0f, 50.0f);
        AddTask(new Vector3(xLoc, 0, yLoc),null);
    }

    private void SetupTask()
    {
        TaskObject taskObjectOverride = null;
        CheckForListOverride(ref taskObjectOverride);

        SwitchSetup(taskObjectOverride != null ? taskObjectOverride : _CurrentTask);
    }

    private void UpdateTask()
    {
        TaskObject taskObjectOverride = null;
        CheckForListOverride(ref taskObjectOverride);

        bool taskDone = SwitchUpdate(taskObjectOverride != null ? taskObjectOverride : _CurrentTask);

        if(taskDone)
        {
            if(_CurrentTask.CurrentTaskType == TaskType.LIST)
            {
                //Individual callback
                taskObjectOverride?.FinishCallback?.Invoke();
                //Move to next one, returns true if just a list
                if(!((TaskObject_List)_CurrentTask).UpdateIndex())
                {
                    SetupTask();
                }
                else
                {
                    _CurrentTask.FinishCallback?.Invoke();
                    _CurrentTask = null;
                }    
            }
            else
            {
                _CurrentTask.FinishCallback?.Invoke();
                _CurrentTask = null;
            }

        }
    }

    private void CheckForListOverride(ref TaskObject taskObjectOverride)
    {
        if (_CurrentTask.CurrentTaskType == TaskType.LIST)
        {
            //Double cast, not great
            taskObjectOverride = ((TaskObject_List)_CurrentTask).TaskObjectLoop[((TaskObject_List)_CurrentTask).TaskIndex];
        }
    }

    private bool SwitchUpdate(TaskObject taskObject)
    {
        bool taskDone = false;
        switch (taskObject.CurrentTaskType)
        {
            case TaskType.MOVE:
                //double cast!!
                TaskObject_Move tom = (TaskObject_Move)taskObject;
                if (Vector3.Distance(tom.Destination, _BaseUnitRef.transform.position) < tom.StoppingDistance)
                {
                    taskDone = true;
                }
                else
                {
                    //Update destination to account for following object
                    if(tom.FollowObject)
                    {
                        tom.Destination = tom.FollowObject.transform.position;
                        _MoveToCallback?.Invoke(tom.Destination);
                    }
                }
                break;
            case TaskType.WAIT:
                if (DateTime.Now >= ((TaskObject_Wait)taskObject).EndTime)
                {
                    taskDone = true;
                }
                break;
            case TaskType.WORK:
                if (((Unit_Worker)_BaseUnitRef).IsFull())
                {
                    taskDone = true;
                }
                break;
            case TaskType.ATTACK:
                TaskObject_Attack ta = ((TaskObject_Attack)taskObject);
                if ((DateTime.Now - ta.LastAttack).TotalSeconds >= ta.TimeBetweenAttacks)
                {
                    if(ta.Enemy)
                    {
                        ta.Enemy.TakeDamage(ta.DamageAmt);
                        ta.LastAttack = DateTime.Now;
                    }
                    else
                    {
                        taskDone = true;
                    }                
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
                ((Unit_Worker)_BaseUnitRef).StartWork();
                break;
            case TaskType.ATTACK:
                _BaseUnitRef.StopAllMovement();
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
    public Transform FollowObject;
    public float StoppingDistance;
}

public class TaskObject_Work:TaskObject
{
    public BaseWorkable Workable;
}

public class TaskObject_Attack : TaskObject
{
    public BaseUnit Enemy;
    public int DamageAmt;
    public float TimeBetweenAttacks;
    public DateTime LastAttack = DateTime.MinValue;
}

public class TaskObject_List : TaskObject
{
    public int TaskIndex = 0;
    public bool ShouldLoop;
    public List<TaskObject> TaskObjectLoop;

    public bool UpdateIndex()
    {
        ++TaskIndex;
        if(TaskIndex >= TaskObjectLoop.Count)
        {
            if(ShouldLoop)
            {
                TaskIndex = 0;
                return false;
            }
            return true;
        }
        return false;
    }
}
