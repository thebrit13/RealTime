using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    SPAWNER,
    DROPOFF
}

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private UnitManager _UnitManager;
    [SerializeField] private List<BaseBuilding> _Buildings;

    private BaseBuilding _CreatedBaseBuilding;

    private Vector3 _MouseWorldPos;

    private List<BaseBuilding> _CreatedBuildings = new List<BaseBuilding>();

    //private RaycastHit _MouseHit;

    private void Awake()
    {
        EventManager.OnClickCreateBuilding += CreateBuildingAtWorldPos;
        EventManager.GetClosestDropOff += GetClosestDropOffLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if(_CreatedBaseBuilding)
        {
            _CreatedBaseBuilding.transform.position = _MouseWorldPos;
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(_CreatedBaseBuilding.gameObject);
            }
        }
    }

    public void PlaceBuilding()
    {
        _CreatedBaseBuilding.Setup(1,BuildingType.SPAWNER);
        _CreatedBuildings.Add(_CreatedBaseBuilding);
        _CreatedBaseBuilding = null;
    }

    public void CreateBuildingAtWorldPos(string prefabName)
    {
        BaseBuilding buildingToCreate = _Buildings.Find(o => o.name == prefabName);

        if(buildingToCreate)
        {
            _CreatedBaseBuilding = Instantiate<BaseBuilding>(buildingToCreate);
        }

        
    }

    private void OnDestroy()
    {
        EventManager.OnClickCreateBuilding -= CreateBuildingAtWorldPos;
        EventManager.GetClosestDropOff -= GetClosestDropOffLocation;
    }

    public void SetMouseWorld(Vector3 pos)
    {
        pos.y = 0;
        _MouseWorldPos = pos;
    }

    public bool IsPlacing()
    {
        return _CreatedBaseBuilding;
    }

    public Vector3 GetClosestDropOffLocation(Vector3 workableLocation)
    {
        List<BaseBuilding> eligibleBuildings = _CreatedBuildings.FindAll(o => o.IsDropOffPoint);
        eligibleBuildings.Sort((x, y) => Vector3.Distance(x.transform.position, workableLocation).CompareTo(Vector3.Distance(y.transform.position, workableLocation)));
        return (eligibleBuildings.Count > 0 ? eligibleBuildings[0].transform.position : workableLocation);
    }
}
