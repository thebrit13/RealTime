using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private List<BaseUnit> _Units;

    private Dictionary<int, List<BaseUnit>> _CreatedUnits = new Dictionary<int, List<BaseUnit>>();
    //private List<BaseUnit> _CreatedUnits = new List<BaseUnit>();

    private void Awake()
    {
        EventManager.OnUnitDeath += OnUnitDeath;
        EventManager.OnCreateUnit += CreateUnit;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void CreateUnit(Data.Unit unitData,Vector3 loc,int teamNumber)
    {
        BaseUnit bu = _Units.Find(o => o.name == unitData.PrefabName);
        if(bu)
        {
            BaseUnit buCreated = Instantiate(bu, loc, Quaternion.identity);
            buCreated.Setup(teamNumber, unitData.Health,unitData.Damage);

            if(!_CreatedUnits.ContainsKey(teamNumber))
            {
                _CreatedUnits.Add(teamNumber, new List<BaseUnit>());
            }
            _CreatedUnits[teamNumber].Add(buCreated);
        }    
    }

    public void MoveSelectedCallback(List<BaseUnit> selectedUnits,Vector3 pos,Transform followObject)
    {
        foreach (BaseUnit bu in selectedUnits)
        {
            bu.MoveTo(pos,followObject);
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

    public List<BaseUnit> GetCreatedUnits(int teamNumber)
    {
        return _CreatedUnits.ContainsKey(teamNumber)?_CreatedUnits[teamNumber]:new List<BaseUnit>();
    }

    private void OnUnitDeath(BaseUnit bu)
    {
        if(!bu)
        {
            return;
        }
        _CreatedUnits[bu.Team].Remove(bu);

        if (bu.Team != 1 && _CreatedUnits[bu.Team].Count == 0)
        {
            EventManager.WaveComplete?.Invoke();
        }
    }

    private void OnDestroy()
    {
        EventManager.OnUnitDeath -= OnUnitDeath;
        EventManager.OnCreateUnit -= CreateUnit;
    }
}
