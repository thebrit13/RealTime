using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool OverUI;

    [Header("Info UI")]
    [SerializeField] private UnitInfo_UI _UnitInfoUI;
    [SerializeField] private UnitInfoMult_UI _UnitInfoMulitUI;

    [Header("Building UI")]
    [SerializeField] private UnitBuilding_UI _UnitBuildingUI;

    private GameObject _ActiveInfoUI = null;

    private void Awake()
    {
        Instance = this;

        HideAllInfo();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            ShowInfo_Building();
        }
    }

    #region ShowUI
    private void ShowUI_Master(GameObject uiObject)
    {
        HideInfo();

        uiObject.SetActive(true);
        _ActiveInfoUI = uiObject;
    }

    public UnitInfo_UI ShowInfo_Unit()
    {
        ShowUI_Master(_UnitInfoUI.gameObject);

        return _UnitInfoUI;
    }

    public UnitInfoMult_UI ShowInfo_UnitMult()
    {
        ShowUI_Master(_UnitInfoMulitUI.gameObject);

        return _UnitInfoMulitUI;
    }

    public UnitBuilding_UI ShowInfo_Building()
    {
        ShowUI_Master(_UnitBuildingUI.gameObject);

        _UnitBuildingUI.Set();

        return _UnitBuildingUI;
    }
    #endregion

    public void HideInfo()
    {
        if (_ActiveInfoUI)
        {
            _ActiveInfoUI.SetActive(false);
        }
    }

    private void HideAllInfo()
    {
        //Unit
        _UnitInfoUI.gameObject.SetActive(false);
        _UnitInfoMulitUI.gameObject.SetActive(false);

        //Building
        _UnitBuildingUI.gameObject.SetActive(false);
    }

    public static void RemovedChildren(Transform transform)
    {
        for(int i = 0;i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
