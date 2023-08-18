using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_UI : Base_UI
{
    [SerializeField] private Transform _Content;
    [SerializeField] private Building_Object_UI _UnitObject;


    public void Set()
    {
        UIManager.RemovedChildren(_Content);
        List<Data.Building> Buildings = PlayerManager.Instance.GetBuildings();
        if(Buildings != null)
        {
            foreach(Data.Building building in Buildings)
            {
                Instantiate<Building_Object_UI>(_UnitObject, _Content).Set(building.BuildingName,delegate
                {
                    EventManager.OnClickCreateBuilding?.Invoke(building.PrefabName);
                    OnClickClose();
                });
            }
        }
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideInfo();
    }
}
