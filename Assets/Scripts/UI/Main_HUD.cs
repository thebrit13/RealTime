using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;

public class Main_HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _MainResource1;

    private System.Action _BuildButtonCallback;

    public void Setup(System.Action buildButtonCallback)
    {
        _BuildButtonCallback = buildButtonCallback;

        SetResourceText(0);
    }

    public void SetResourceText(int newAmt)
    {
        _MainResource1.SetText(string.Format("Metal: {0}",newAmt));
    }

    public void OnClickBuildButton()
    {
        _BuildButtonCallback?.Invoke();
    }
}
