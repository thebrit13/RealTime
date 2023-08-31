using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _SelectionLine;
    [SerializeField] private UnitManager _UnitManagerRef;
    [SerializeField] private BuildingManager _BuildingManagerRef;

    Vector3 _MouseWorldPos;

    private RaycastHit _MouseHit;

    private Vector3 _MouseDown;

    private bool _SelectionInProgress = false;

    private BaseSelectable _WorldMouseUnitHit = null;
    private BaseSelectable _WorldMouseEnemyUnitHit = null;
    private BaseWorkable _WorldMouseWorkableHit = null;

    private List<BaseUnit> _SelectedUnits = new List<BaseUnit>();

    private const int TEAM_NUMBER = 1;

    private void Awake()
    {
        _SelectionLine.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Not sure if this should be checked this much
        if (UIManager.Instance.OverUI)
        {
            return;
        }

        if(_BuildingManagerRef?.IsPlacing() == true)
        {
            SetMouseWorld(false,false);
            _BuildingManagerRef?.SetMouseWorld(_MouseWorldPos);
            if (Input.GetMouseButtonDown(0))
            {
                _BuildingManagerRef?.PlaceBuilding();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SetMouseWorld(true,false);
            SelectionBegin();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectionEnd();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(_SelectedUnits?.Count > 0)
            {
                SetMouseWorld(true, true,true);
                if (_WorldMouseWorkableHit != null)
                {
                    _UnitManagerRef?.StartWorkCallback(_SelectedUnits, _WorldMouseWorkableHit);
                }
                else
                {
                    _UnitManagerRef?.MoveSelectedCallback(_SelectedUnits, _MouseWorldPos, _WorldMouseEnemyUnitHit?.transform);
                }
            }
        }

        if (_SelectionInProgress)
        {
            SetMouseWorld(false,false);
            UpdateSelectionLine();
        }
    }

    private void SetMouseWorld(bool checkForUnit,bool checkForWorkable,bool rmb = false)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _MouseHit))
        {
            _MouseWorldPos = _MouseHit.point;

            //Enemy only
            _WorldMouseEnemyUnitHit = null;

            //LMB - check for unit to select
            //RMB - check for unit to attack
            if (checkForUnit)
            {
                if (_MouseHit.transform.tag == "Unit" || _MouseHit.transform.tag == "Building")
                {
                    BaseSelectable tempBS = _MouseHit.transform.GetComponent<BaseSelectable>();
                    if(tempBS?.Team == TEAM_NUMBER)
                    {
                        _WorldMouseUnitHit = tempBS;
                    }
                    else if(rmb)
                    {
                        _WorldMouseEnemyUnitHit = tempBS;
                    }
                }
                else
                {
                    if(!rmb)
                    {
                        _WorldMouseUnitHit?.SetSelectionState(UnitSelectionState.NONE, false);
                        _WorldMouseUnitHit = null;
                    }
                }
            }

            //RMB - check for tree/mine to work
            if(checkForWorkable)
            {
                if (_MouseHit.transform.tag == "Workable")
                {
                    _WorldMouseWorkableHit = _MouseHit.transform.parent.GetComponent<BaseWorkable>();
                }
                else
                {
                    _WorldMouseWorkableHit = null;
                }
            }
        }
    }

    private void SelectionBegin()
    {
        _MouseDown = _MouseWorldPos;
        _SelectionInProgress = true;
        _SelectionLine.gameObject.SetActive(true);
    }

    private void UpdateSelectionLine()
    {
        _SelectionLine.SetPosition(0, _MouseDown + new Vector3(0, 1, 0));
        _SelectionLine.SetPosition(1, new Vector3(_MouseDown.x, _MouseDown.y + 1, _MouseWorldPos.z));
        _SelectionLine.SetPosition(2, _MouseWorldPos + new Vector3(0, 1, 0));
        _SelectionLine.SetPosition(3, new Vector3(_MouseWorldPos.x, _MouseDown.y + 1, _MouseDown.z));
    }

    private void SelectionEnd()
    {
        _SelectionInProgress = false;
        _SelectionLine.gameObject.SetActive(false);
        ClearSelectedUnits();

        //single selection
        if(_WorldMouseUnitHit)
        {
            if (_WorldMouseUnitHit.GetType().IsSubclassOf(typeof(BaseUnit)))
            {
                _WorldMouseUnitHit.SetSelectionState(UnitSelectionState.SELECTED, true);
                _SelectedUnits.Add(_WorldMouseUnitHit.GetComponent<BaseUnit>());
                UIManager.Instance?.ShowInfo_Unit()?.Set(_SelectedUnits[0]);
                return;
            }
            else if (_WorldMouseUnitHit.GetType() == typeof(BaseBuilding) || _WorldMouseUnitHit.GetType().IsSubclassOf(typeof(BaseBuilding)))
            {
                _WorldMouseUnitHit.SetSelectionState(UnitSelectionState.SELECTED, true);
                return;
            }
        }

        //Multi select for units only
        Vector3 topLeft = new Vector3(Mathf.Min(_MouseDown.x, _MouseWorldPos.x), 0, Mathf.Max(_MouseDown.z, _MouseWorldPos.z));
        Vector3 bottomRight = new Vector3(Mathf.Max(_MouseDown.x, _MouseWorldPos.x), 0, Mathf.Min(_MouseDown.z, _MouseWorldPos.z));

        foreach (BaseUnit bu in _UnitManagerRef?.GetCreatedUnits(TEAM_NUMBER))
        {
            if (IsInSelection(bu.transform.position, topLeft, bottomRight))
            {
                bu.SetSelectionState(UnitSelectionState.SELECTED,false);
                _SelectedUnits.Add(bu);
            }
        }

        if (_SelectedUnits.Count == 1)
        {
            UIManager.Instance?.ShowInfo_Unit()?.Set(_SelectedUnits[0]);
        }
        else if (_SelectedUnits.Count > 1)
        {
            UIManager.Instance?.ShowInfo_UnitMult()?.Set(_SelectedUnits);
        }
    }

    private void ClearSelectedUnits()
    { 
        foreach (BaseUnit bu in _SelectedUnits)
        {
            bu.SetSelectionState(UnitSelectionState.NONE,false);
        }
        _SelectedUnits.Clear();
        UIManager.Instance?.HideInfo();
    }

    private bool IsInSelection(Vector3 testLoc, Vector3 topLeft, Vector3 bottomRight)
    {
        bool inside = true;
        inside = inside && testLoc.x > topLeft.x;
        inside = inside && testLoc.x < bottomRight.x;
        inside = inside && testLoc.z < topLeft.z;
        inside = inside && testLoc.z > bottomRight.z;

        return inside;
    }
}
