using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    #region Variables


    #region LinkedTabScripts
    public FinanceManager financeManager;

    #endregion

    [Header("Pannel")]
    public GameObject MainTabPanel;  // Balance, General
    public GameObject ShopPannel; 
    public GameObject EmployeePannel;

    [Header("Menu UIs")]
    public TextMeshProUGUI balanceTxt;
    public TextMeshPro time;


    public Button close;

    private decimal BusinessBalance;


    #endregion

    public static MenuManager Instance;
    #region Unity Methods
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(Instance);
        }
        Initialize();
    }

    private void OnEnable() {
        EventManager.Subscribe("TabToggle", HandleTabToggle);
    }

    private void OnDisable() {
        EventManager.Unsubscribe("TabToggle", HandleTabToggle);
    }

    #endregion

    void Initialize() {
        LinkScripts();
        HideTab();
    }

    void LinkScripts() {
        if(financeManager == null)
            financeManager = FindFirstObjectByType<FinanceManager>();

    }

    #region Helper Methods

    private void HandleTabToggle(bool isTabOpen) {
        if(isTabOpen) {  
            ShowTab();
        } else {
            HideTab();
        }
    }


    public void ShowTab() {
        MainTabPanel.SetActive(true);
        BusinessBalance = financeManager.GetBusinessBalance();
        balanceTxt.text = "$ " + BusinessBalance.ToString();
    }

    public void HideTab() {
        MainTabPanel.SetActive(false);
        Btn_CloseShop();

    }

    public void Btn_OpenShop() {
        ShopPannel.SetActive(true);
    }
    public void Btn_CloseShop() {
        ShopPannel.SetActive(false );
    }

    #endregion

}
