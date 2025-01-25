using TMPro;
using UnityEngine;

public class TabUI : MonoBehaviour {
    #region Variables


    #region LinkedTabScripts
    public FinanceManager financeManager;
    #endregion

    public GameObject MainTabPanel;  // Balance, General
    public TextMeshProUGUI balanceTxt;

    private decimal BusinessBalance;


    #endregion

    #region Unity Methods
    private void Awake() {
        Initialize();
    }

    private void Start() {
        HideTab();
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
        balanceTxt.text = "$ "+ BusinessBalance.ToString();
    }

    public void HideTab() {
        MainTabPanel.SetActive(false);
    }


    #endregion
}
