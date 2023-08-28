using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;



    [HideInInspector]
    public bool OverUI;

    [SerializeField] private Main_HUD _MainHUD;

    [Header("Info UI")]
    [SerializeField] private UnitInfo_UI _UnitInfoUI;
    [SerializeField] private UnitInfoMult_UI _UnitInfoMulitUI;

    [Header("Building UI")]
    [SerializeField] private Building_UI _BuildingMenuUI;
    [SerializeField] private BuildingInfo_UI _BuildingInfoUI;

    private GameObject _ActiveUI = null;

    private void Awake()
    {
        Instance = this;

        HideAllInfo();
        _MainHUD.Setup(Show_BuildingMenu);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region ShowUI
    private void ShowUI_Master(GameObject uiObject)
    {
        HideInfo();

        uiObject.SetActive(true);
        _ActiveUI = uiObject;
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

    public void Show_BuildingMenu()
    {
        ShowUI_Master(_BuildingMenuUI.gameObject);

        _BuildingMenuUI.Set();
    }

    public BuildingInfo_UI ShowInfo_Building(System.Action<Data.Unit> onClickOption)
    {
        ShowUI_Master(_BuildingInfoUI.gameObject);

        _BuildingInfoUI.Set(onClickOption);

        return _BuildingInfoUI;
    }
    #endregion

    public void HideInfo()
    {
        _ActiveUI?.SetActive(false);
    }

    private void HideAllInfo()
    {
        //Unit
        _UnitInfoUI.gameObject.SetActive(false);
        _UnitInfoMulitUI.gameObject.SetActive(false);

        //Building
        _BuildingMenuUI.gameObject.SetActive(false);
        _BuildingInfoUI.gameObject.SetActive(false);
    }

    public static void RemovedChildren(Transform transform)
    {
        for(int i = 0;i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SetResourceText(int amt)
    {
        _MainHUD.SetResourceText(amt);
    }
}
