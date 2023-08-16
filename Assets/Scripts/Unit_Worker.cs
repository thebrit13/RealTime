using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Worker : BaseUnit
{
    private const int MAX_RESOURCE_COUNT = 10;
    private const float WORK_RESOURCE_TICK_TIME = .5f;

    private int _CurrentResourceCount;

    private Coroutine _WorkCo;

    public void StartWorkTask(BaseWorkable bw)
    {
        _TaskManager.AddTask(bw,StopWork,DropOffResources);
    }

    public void StartWork()
    {
        if(_WorkCo != null)
        {
            StopCoroutine(_WorkCo);
        }
        _WorkCo = StartCoroutine(WorkCoroutine());
    }

    public void StopWork()
    {
        if (_WorkCo != null)
        {
            StopCoroutine(_WorkCo);
        }
    }

    IEnumerator WorkCoroutine()
    {
        while(true)
        {
            ++_CurrentResourceCount;
            yield return new WaitForSeconds(WORK_RESOURCE_TICK_TIME);
        }
    }

    //Will want this to be dropped at a building
    public void DropOffResources()
    {
        PlayerManager.Instance.AddResource(ResourceType.METAL, _CurrentResourceCount);
        _CurrentResourceCount = 0;
    }

    public bool IsFull()
    {
        return _CurrentResourceCount >= MAX_RESOURCE_COUNT;
    }
}
