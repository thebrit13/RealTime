using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool OverUI;

    [Header("Info UI")]
    [SerializeField] private UnitInfo_UI _UnitInfoUI;
    [SerializeField] private UnitInfoMult_UI _UnitInfoMulitUI;

    [Header("Building UI")]
    [SerializeField] private Building_UI _BuildingUI;
    [SerializeField] private BuildingInfo_UI _BuildingInfoUI;

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
            Show_Building();
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

    public Building_UI Show_Building()
    {
        ShowUI_Master(_BuildingUI.gameObject);

        _BuildingUI.Set();

        return _BuildingUI;
    }

    public BuildingInfo_UI ShowInfo_Building(System.Action<string> createUnitCallback)
    {
        ShowUI_Master(_BuildingInfoUI.gameObject);

        _BuildingInfoUI.Set(createUnitCallback);

        return _BuildingInfoUI;
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
        _BuildingUI.gameObject.SetActive(false);
        _BuildingInfoUI.gameObject.SetActive(false);
    }

    public static void RemovedChildren(Transform transform)
    {
        for(int i = 0;i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
