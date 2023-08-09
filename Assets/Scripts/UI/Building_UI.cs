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
        Instantiate<Building_Object_UI>(_UnitObject, _Content).Set(delegate
        {
            EventManager.OnClickCreateBuilding?.Invoke();
            OnClickClose();
        });
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideInfo();
    }
}
