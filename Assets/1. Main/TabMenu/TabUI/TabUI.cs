using TMPro;
using UnityEngine;

public class TabUI : MonoBehaviour {
    #region Variables

    public GameObject MainTabPanel;
    public GameObject balanceUi;
    public TextMeshProUGUI balanceTxt;

    public decimal businessBalance;
    public FinanceManager financeManager;

    #endregion

    #region Unity Methods

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
        businessBalance = financeManager.GetBusinessBalance();
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
    }

    public void HideTab() {
        MainTabPanel.SetActive(false);
    }

    #endregion
}
