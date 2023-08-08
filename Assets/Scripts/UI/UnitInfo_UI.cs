using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitInfo_UI : Base_UI
{
    [SerializeField] TextMeshProUGUI _Name;
    [SerializeField] TextMeshProUGUI _Attack;
    [SerializeField] TextMeshProUGUI _Health;

    public void Set(BaseUnit bUnit)
    {
        //_Name.SetText(name);
        //_Attack.SetText(attack);
        //_Health.SetText(health);
    }
}
