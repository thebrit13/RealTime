using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private BaseBuilding _BaseBuilding;

    private BaseBuilding _CreatedBaseBuilding;

    private Vector3 _MouseWorldPos;

    //private RaycastHit _MouseHit;

    private void Awake()
    {
        EventManager.OnClickCreateBuilding += CreateBuildingAtWorldPos;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(_CreatedBaseBuilding)
        {
            _CreatedBaseBuilding.transform.position = _MouseWorldPos;
        }


    }

    public void PlaceBuilding()
    {
        _CreatedBaseBuilding.Setup();
        _CreatedBaseBuilding = null;
    }

    public void CreateBuildingAtWorldPos()
    {
        _CreatedBaseBuilding = Instantiate<BaseBuilding>(_BaseBuilding);
    }

    private void OnDestroy()
    {
        EventManager.OnClickCreateBuilding -= CreateBuildingAtWorldPos;
    }

    public void SetMouseWorld(Vector3 pos)
    {
        _MouseWorldPos = pos;
    }

    public bool IsPlacing()
    {
        return _CreatedBaseBuilding;
    }

    ////Should consolidate so we arent doing this in two places
    //private void SetMouseWorld()
    //{
    //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _MouseHit))
    //    {
    //        _MouseWorldPos = _MouseHit.point;
    //        _MouseWorldPos.y = 0;
    //    }
    //}

}
