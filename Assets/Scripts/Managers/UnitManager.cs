using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private List<BaseUnit> _Units;

    private List<BaseUnit> _CreatedUnits = new List<BaseUnit>();

    private void Awake()
    {
        EventManager.OnUnitDeath += OnUnitDeath;
        EventManager.OnCreateUnit += CreateUnit;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void CreateUnit(Data.Unit unitData,Vector3 loc)
    {
        BaseUnit bu = _Units.Find(o => o.name == unitData.PrefabName);
        if(bu)
        {
            BaseUnit buCreated = Instantiate(bu, loc, Quaternion.identity);
            buCreated.Setup(1,unitData.Health);
            _CreatedUnits.Add(buCreated);
        }    
    }

    public void MoveSelectedCallback(List<BaseUnit> selectedUnits,Vector3 pos)
    {
        foreach (BaseUnit bu in selectedUnits)
        {
            bu.MoveTo(pos);
        }
    }

    public void StartWorkCallback(List<BaseUnit> workers, BaseWorkable bw)
    {
        foreach (BaseUnit bu in workers)
        {
            if (bu.GetType() == typeof(Unit_Worker))
            {
                ((Unit_Worker)bu).StartWorkTask(bw);
            }         
        }
    }

    public List<BaseUnit> GetCreatedUnits()
    {
        return _CreatedUnits;
    }

    private void OnUnitDeath(BaseUnit bu)
    {
        if(!bu)
        {
            return;
        }

        _CreatedUnits.Remove(bu);
    }

    private void OnDestroy()
    {
        EventManager.OnUnitDeath -= OnUnitDeath;
        EventManager.OnCreateUnit -= CreateUnit;
    }
}
