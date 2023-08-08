using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : BaseSelectable
{
    [SerializeField] private NavMeshAgent _NMA;

    public void MoveTo(Vector3 dest)
    {
        _NMA.SetDestination(dest);
    }
}
