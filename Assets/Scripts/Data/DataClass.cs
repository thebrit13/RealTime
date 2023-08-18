using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Data
{
    [Serializable]
    public class DataClass
    {
        public Buildings Buildings;
        public Units Units;
    }

    [Serializable]
    public class Buildings
    {
        public List<Building> BuildingsList;
    }

    [Serializable]
    public class Building
    {
        public string BuildingName;
        public string PrefabName;
    }

    [Serializable]
    public class Units
    {
        public List<Unit> UnitList;
    }

    [Serializable]
    public class Unit
    {

    }
}


