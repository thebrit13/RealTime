using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private BaseUnit _BaseUnit;

    private List<BaseUnit> _CreatedUnits = new List<BaseUnit>();

    private void Awake()
    {
        EventManager.OnUnitDeath += OnUnitDeath;
    }

    // Start is called before the first frame update
    void Start()
    {
        _CreatedUnits.Add(Instantiate(_BaseUnit, Vector3.zero, Quaternion.identity));
    }

    public void CreateUnit(string id,Vector3 loc)
    {
        _CreatedUnits.Add(Instantiate(_BaseUnit, loc, Quaternion.identity));
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
            ((Unit_Worker)bu).StartWork(bw);
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
    }
}
