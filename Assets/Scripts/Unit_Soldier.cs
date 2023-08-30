using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit_Soldier : BaseUnit
{
    private BaseUnit _CurrentTarget;

    private float _AttackDelay = 1.0f;

    //private Coroutine _AttackCo;

    public override void Update()
    {
        base.Update();
        if (_CurrentTarget)
        {
            this.transform.LookAt(_CurrentTarget.transform);
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_CurrentTarget)
        {
            return;
        }

        if (!other.tag.Contains("Unit"))
        {
            return;
        }

        BaseUnit bu = other.transform.parent.GetComponent<BaseUnit>();
        if(bu?.Team != this.Team)
        {
            _TaskManager.AddTask(bu,_AttackDelay, 2);
            _CurrentTarget = bu;
        }

        

        //TEMP
        //BaseUnit tempTarget = other.transform.parent.GetComponent<BaseUnit>();
        //if (tempTarget)
        //{
        //    _CurrentTarget = tempTarget;
        //    if (_AttackCo != null)
        //    {
        //        StopCoroutine(_AttackCo);
        //    }
        //    _AttackCo = StartCoroutine(Attack());
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (_CurrentTarget == null)
        {
            return;
        }

        if (!other.tag.Contains("Unit"))
        {
            return;
        }

        if (other.gameObject == _CurrentTarget.gameObject)
        {
            _TaskManager.ClearAllTasks();
            _CurrentTarget = null;
        }
    }

    //IEnumerator Attack()
    //{
    //    _NMA.ResetPath();
    //    while (_CurrentTarget)
    //    {
    //        _CurrentTarget.TakeDamage(2);
    //        yield return new WaitForSeconds(_AttackDelay);
    //    }
    //}
}
