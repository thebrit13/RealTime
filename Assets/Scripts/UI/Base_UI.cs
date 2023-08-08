using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Base_UI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler 
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.OverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.OverUI = false;
    }

    private void OnDisable()
    {
        UIManager.Instance.OverUI = false;
    }
}
