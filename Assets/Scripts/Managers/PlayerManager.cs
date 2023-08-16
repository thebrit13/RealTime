using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    METAL
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private Dictionary<ResourceType, int> _Resources;

    private void Awake()
    {
        Instance = this;
        _Resources = new Dictionary<ResourceType, int>();
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
    }
}
