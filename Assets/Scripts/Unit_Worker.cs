using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Worker : BaseUnit
{
    private const int MAX_RESOURCE_COUNT = 10;

    private int _CurrentResourceCount;

    public void StartWork(BaseWorkable bw)
    {
        _TaskManager.AddTask(bw);
    }

    public bool IsFull()
    {
        return _CurrentResourceCount >= MAX_RESOURCE_COUNT;
    }

    public void AddResource(int amt)
    {
        ++_CurrentResourceCount;
    }
}
