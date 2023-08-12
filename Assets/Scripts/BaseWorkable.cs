using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkable : MonoBehaviour
{
    private const int MAX_WORKERS = 4;

    private List<Unit_Worker> _CurrentWorkers;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
