using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo_UI : MonoBehaviour
{
    [SerializeField] private BuildingUnit_Object_UI _UnitObject;
    [SerializeField] private Transform _Content;

   // private System.Action<string> _UnitSpawnCallback;


    public void Set(System.Action<string> unitSpawnCallback)
    {
        UIManager.RemovedChildren(_Content);

        Instantiate<BuildingUnit_Object_UI>(_UnitObject, _Content).Set("temp", unitSpawnCallback);
    }

    public void OnClickClose()
    {
        UIManager.Instance.HideInfo();
    }
}
