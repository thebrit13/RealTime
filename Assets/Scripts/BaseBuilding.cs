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

    private System.Action<string,Vector3> _CreateUnitCallback;

    public override void Awake()
    {
        base.Awake();
        _NMO.enabled = false;
        _Collider.enabled = false;
    }

    public void Setup(int teamNumber,System.Action<string,Vector3> createUnitCallback)
    {
        base.Setup(teamNumber);
        _NMO.enabled = true;
        _Collider.enabled = true;
        OnClick += OnClickFunc;

        _CreateUnitCallback = createUnitCallback;
    }

    private void OnClickFunc()
    {
        UIManager.Instance?.ShowInfo_Building(CreateUnit);
    }

    private void OnDestroy()
    {
        OnClick -= OnClickFunc;
    }

    private void CreateUnit(string id)
    {
        _CreateUnitCallback?.Invoke(id, _UnitSpawnLocation.position);
    }
}
