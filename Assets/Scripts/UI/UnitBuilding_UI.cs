using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuilding_UI : Base_UI
{
    [SerializeField] private Transform _Content;
    [SerializeField] private UnitBuilding_Object_UI _UnitObject;


    public void Set()
    {
        UIManager.RemovedChildren(_Content);
        Instantiate<UnitBuilding_Object_UI>(_UnitObject, _Content).Set(delegate
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
