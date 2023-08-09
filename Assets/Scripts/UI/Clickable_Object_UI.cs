using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_Object_UI : MonoBehaviour
{
    private System.Action _ClickCallback;
    private System.Action<string> _ClickCallbackString;

    private string _ID;

    public void Set(System.Action clickCallback)
    {
        _ClickCallback = clickCallback;
    }

    public void Set(string id,System.Action<string> clickCallback)
    {
        _ID = id;
        _ClickCallbackString = clickCallback;
    }

    public void OnClick()
    {
        _ClickCallback?.Invoke();
        _ClickCallbackString?.Invoke(_ID);
    }
}
