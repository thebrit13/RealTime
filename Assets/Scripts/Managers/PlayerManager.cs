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

    public static PlayerManager Instance;

    private Dictionary<ResourceType, int> _Resources;

    private DataManager _DataManager;


    private void Awake()
    {
        Instance = this;
        _Resources = new Dictionary<ResourceType, int>();
        _DataManager = new DataManager(_DataAsset.text);
    }

    private void Start()
    {
        _DataManager.LoadData();

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
}
