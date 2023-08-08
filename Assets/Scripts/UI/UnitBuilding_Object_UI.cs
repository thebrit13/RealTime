using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuilding_Object_UI : MonoBehaviour
{
    private System.Action _ClickCallback;

    public void Set(System.Action clickCallback)
    {
        _ClickCallback = clickCallback;
    }

    public void OnClick()
    {
        _ClickCallback?.Invoke();
    }
}
