using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataManager
    {
        public DataClass DataClass;

        private string _JSONData;
        public DataManager(string JsonData)
        {
            _JSONData = JsonData;
        }

        public IEnumerator LoadData()
        {
            DataClass = JsonUtility.FromJson<DataClass>(_JSONData);
            yield return null;
        }
    }
}


