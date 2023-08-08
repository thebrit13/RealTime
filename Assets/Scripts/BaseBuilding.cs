using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseBuilding : BaseSelectable
{
    [SerializeField] private NavMeshObstacle _NMO;
    [SerializeField] private Collider _Collider;

    public override void Awake()
    {
        base.Awake();
        _NMO.enabled = false;
        _Collider.enabled = false;
    }

    public void Setup()
    {
        _NMO.enabled = true;
        _Collider.enabled = true;
    }
}
