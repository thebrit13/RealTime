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

        _TaskManager = new TaskManager();
        _TaskManager.Setup(this, MoveToInternal);
    }

    public virtual void Update()
    {
        _TaskManager?.UpdateTaskManager();
    }

    public void MoveTo(Vector3 dest)
    {
        _TaskManager.AddTask(dest);
    }

    private void MoveToInternal(Vector3 dest)
    {
        _NMA.SetDestination(dest);
    }
   

    private void OnDestroy()
    {
        EventManager.OnUnitDeath?.Invoke(this);
    }
}
