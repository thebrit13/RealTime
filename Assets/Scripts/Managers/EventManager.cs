using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static System.Action<Data.Building> OnClickCreateBuilding;

    public static System.Action<BaseUnit> OnUnitDeath;

    public static System.Action<Data.Unit, Vector3> OnCreateUnit;

    public delegate Vector3 GetClosestDropOffDel(Vector3 unitPos);

    public static GetClosestDropOffDel GetClosestDropOff;

    public static System.Action StartGameLogic;
}
