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
        public List<Wave> Waves;
    }

    #region Buildings
    [Serializable]
    public class Buildings
    {
        public List<Building> BuildingsList;
    }
    #endregion

    [Serializable]
    public class Building
    {
        public string BuildingName;
        public string PrefabName;
        public int Health;
    }

    #region Units
    [Serializable]
    public class Units
    {
        public List<Unit> UnitsList;

        public Unit GetUnitByID(string ID)
        {
            return UnitsList.Find(o => o.ID == ID);
        }
    }

    [Serializable]
    public class Unit
    {
        public string ID;
        public string UnitName;
        public string PrefabName;
        public int Damage;
        public int Health;
    }
    #endregion

    #region Waves
    [Serializable]
    public class Wave
    {
        public int Number;
        public List<WaveUnitInfo> Units;
    }

    [Serializable]
    public class WaveUnitInfo
    {
        public float SpawnTime;
        public string ID;
        public int Amt;
    }
    #endregion
}


