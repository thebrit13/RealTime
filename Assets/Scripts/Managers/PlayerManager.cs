using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public enum ResourceType
{
    METAL
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TextAsset _DataAsset;
    [SerializeField] private WaveManager _WaveManager;

    public static PlayerManager Instance;

    private Dictionary<ResourceType, int> _Resources;

    private DataManager _DataManager;
    public DataManager DataManager { 
        get
        {
            return _DataManager;
        }
    }


    private void Awake()
    {
        Instance = this;
        _Resources = new Dictionary<ResourceType, int>();
        _DataManager = new DataManager(_DataAsset.text);
    }

    private void Start()
    {
        StartCoroutine(LoadDataAndStart());
    }

    IEnumerator LoadDataAndStart()
    {
        Debug.Log("Loading Data");
        yield return _DataManager.LoadData();
        Debug.Log("Finished Loading Data");

        _WaveManager.Setup(_DataManager.DataClass.Waves);

        EventManager.StartGameLogic?.Invoke();
    }

    public void AddResource(ResourceType rt, int amount)
    {
        if (_Resources.ContainsKey(rt))
        {
            _Resources[rt] += amount;
        }
        else
        {
            _Resources.Add(rt, amount);
        }
        UIManager.Instance.SetResourceText(_Resources[rt]);
    }

    public List<Data.Building> GetBuildings()
    {
        return _DataManager.DataClass.Buildings.BuildingsList;
    }

    public List<Data.Unit> GetUnits()
    {
        return _DataManager.DataClass.Units.UnitsList;
    }
}
