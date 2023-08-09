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

    private TaskManager _TaskManager;

    private void Awake()
    {
        _TaskManager = new TaskManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        _CreatedUnits.Add(Instantiate(_BaseUnit,new Vector3(0,0,0),Quaternion.identity));
        _CreatedUnits.Add(Instantiate(_BaseUnit,new Vector3(2,0,0),Quaternion.identity));
        _CreatedUnits.Add(Instantiate(_BaseUnit,new Vector3(4,0,0),Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        _TaskManager?.Update();  
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

    public List<BaseUnit> GetCreatedUnits()
    {
        return _CreatedUnits;
    }
}
