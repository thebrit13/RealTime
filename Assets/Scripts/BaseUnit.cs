using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : BaseSelectable
{
    [SerializeField] protected NavMeshAgent _NMA;

    protected TaskManager _TaskManager;

    public override void Awake()
    {
        base.Awake();
        //to active nav mesh agent when spawned, might not need
        _NMA.SetDestination(this.transform.position + this.transform.forward);


    }

    public override void Setup(int teamNumber, int health, int damage)
    {
        base.Setup(teamNumber, health, damage);
        _TaskManager = new TaskManager();
        _TaskManager.Setup(this,teamNumber != 1, MoveToInternal);
    }

    public virtual void Update()
    {
        _TaskManager?.UpdateTaskManager();
    }

    public void MoveTo(Vector3 dest,Transform followObjects)
    {
        _TaskManager.AddTask(dest, followObjects);
    }

    private void MoveToInternal(Vector3 dest)
    {
        _NMA.SetDestination(dest);
    }
   

    private void OnDestroy()
    {
        EventManager.OnUnitDeath?.Invoke(this);
    }

    public void StopAllMovement()
    {
        _NMA.ResetPath();
    }
}
