using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clickable_Object_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ButtonName;

    private System.Action _ClickCallback;
    //private System.Action<string> _ClickCallbackString;

    private string _Name;

    public virtual void Set(System.Action clickCallback)
    {
        _ClickCallback = clickCallback;
    }

    public void Set(string buttonName, System.Action clickCallback)
    {
        _ButtonName.text = buttonName;
        _ClickCallback = clickCallback;
    }

    public void OnClick()
    {
        _ClickCallback?.Invoke();
        //_ClickCallbackString?.Invoke(_ID);
    }
}
