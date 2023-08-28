using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo_UI : MonoBehaviour
{
    [SerializeField] private BuildingUnit_Object_UI _UnitObject;
    [SerializeField] private Transform _Content;

    public void Set(System.Action<Data.Unit> onClickOption)
    {
       UIManager.RemovedChildren(_Content);

        List<Data.Unit> Units = PlayerManager.Instance.GetUnits();
        if (Units != null)
        {
            foreach (Data.Unit unit in Units)
            {
                Instantiate<BuildingUnit_Object_UI>(_UnitObject, _Content).Set(unit.UnitName, delegate ()
                {
                    onClickOption(unit);
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
