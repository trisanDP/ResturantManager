using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    #region Variables

    public static MenuManager Instance;

    #region LinkedTabScripts
    public FinanceManager financeManager;
    #endregion

    [Header("Panel")]
    public GameObject MainTabWindow;
    public GameObject HomePanel;
    public GameObject ShopPanel;
    public GameObject EmployeePanel;

    [Header("Menu UIs")]
    public TextMeshProUGUI balanceTxt;
    public TextMeshPro time;
    public Button close;
    private decimal BusinessBalance;

    #endregion

    #region Unity Methods
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
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
        MainTabWindow.SetActive(true);
        HomePanel.SetActive(true);
        BusinessBalance = financeManager.GetBusinessBalance();
        balanceTxt.text = "$ " + BusinessBalance.ToString();
    }

    public void HideTab() {
        HomePanel.SetActive(false);
        MainTabWindow.SetActive(false);
        Btn_CloseShop();
    }

    public void Btn_OpenShop() {
        ShopPanel.SetActive(true);
    }
    public void Btn_CloseShop() {
        ShopPanel.SetActive(false);
    }
    #endregion
}