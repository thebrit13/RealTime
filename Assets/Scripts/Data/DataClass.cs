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
        public int Health;
    }

    [Serializable]
    public class Units
    {
        public List<Unit> UnitsList;
    }

    [Serializable]
    public class Unit
    {
        public string UnitName;
        public string PrefabName;
        public int Damage;
        public int Health;
    }
}


