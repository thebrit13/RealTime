using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseBuilding : BaseSelectable
{
    [SerializeField] private NavMeshObstacle _NMO;
    [SerializeField] private Collider _Collider;
    [SerializeField] private Transform _UnitSpawnLocation;

    [HideInInspector]
    public bool IsDropOffPoint
    {
        get { return _BuildingType == BuildingType.SPAWNER; }
    }

    private BuildingType _BuildingType;

    public override void Awake()
    {
        base.Awake();
        _NMO.enabled = false;
        _Collider.enabled = false;
    }

    public void Setup(int teamNumber,int health,BuildingType bt)
    {
        base.Setup(teamNumber,health);
        _NMO.enabled = true;
        _Collider.enabled = true;
        OnClick += OnClickFunc;

        _BuildingType = bt;
    }

    private void OnClickFunc()
    {
        UIManager.Instance?.ShowInfo_Building(CreateUnit);
    }

    private void OnDestroy()
    {
        OnClick -= OnClickFunc;
    }

    private void CreateUnit(Data.Unit unitData)
    {
        EventManager.OnCreateUnit?.Invoke(unitData, _UnitSpawnLocation.position);
    }
}
