using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    public static DebugHelper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool UseTeamNumberOverride = false;
    public int TeamNumberOverride = 1;
}
