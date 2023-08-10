using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static System.Action OnClickCreateBuilding;

    public static System.Action<BaseUnit> OnUnitDeath;
}
