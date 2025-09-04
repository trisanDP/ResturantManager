using RestaurantManagement;
using Game_StarterAssets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#region DesktopUIManager Class
public class DesktopUIManager : MonoBehaviour {
    public static DesktopUIManager Instance;

    #region Panels
    [Header("Pannels")]
    public GameObject DesktopWindow;      // Main parent (e.g., DesktopMenu)
    public GameObject DesktopPannel;      // Main desktop panel
    public GameObject TasksPanel;
    public GameObject OrdersPanel;
    public GameObject ShopPanel;
    public GameObject ManagementPanel;
    public GameObject FinancePanel;       // Finance App panel

    #endregion

    #region UI Elements
    [Header("UI Elements")]
    public List<TextMeshProUGUI> balanceText;   // Desktop balance display (always visible)
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI DateTxt;


    private decimal BusinessBalance;

    #endregion

    #region Linked Managers
    [Header("Linked Managers")]
    public FinanceManager financeManager;
    public OrdersUIManager ordersUIManager;
    public QuestUIManager questUIManager;

    InputActionAsset inputAsset;
    #endregion

    #region Unity Methods
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        Initialize();

        // Correctly assign StarterAssetsInputs instead of InputActionAsset
        StarterAssetsInputs starterInputs = FindFirstObjectByType<StarterAssetsInputs>();
        if (starterInputs != null)
            inputAsset = starterInputs.GetComponent<InputActionAsset>();
        else
            Debug.LogWarning("StarterAssetsInputs not found!");
    }

    private void Update() {
        UpdateTimeDisplay();
    }


    private void OnEnable() {
        EventManager.Subscribe("TabToggle", HandleTabToggle);
        if(financeManager != null) {
            financeManager.OnBusinessBalanceChanged += UpdateDesktopBalance;
        } else {
            Debug.LogWarning("FinanceManager is null in OnEnable!");
        }
    }

    private void OnDisable() {
        EventManager.Unsubscribe("TabToggle", HandleTabToggle);
        if(financeManager != null)
            financeManager.OnBusinessBalanceChanged -= UpdateDesktopBalance;
    }
    #endregion

    #region Time
    private void UpdateTimeDisplay() {
        // Get current time in seconds from TimeManager
        float currentSeconds = TimeManager.Instance.currentTimeInSeconds;
        int totalSeconds = Mathf.FloorToInt(currentSeconds);
        int hour = totalSeconds / 3600;
        int minute = (totalSeconds % 3600) / 60;
        int second = totalSeconds % 60;

        // Format as HH:MM:SS (you can modify formatting as needed)
        timeTxt.text = string.Format("{0:00}:{1:00}", hour, minute);
        DateTxt.text = TimeManager.Instance.GetFormattedDate();
    }
    #endregion

    #region Initialization
    void Initialize() {
        LinkScripts();
        HideDesktop();
    }

    void LinkScripts() {
        // Always get the FinanceManager from RestaurantManager
        if (RestaurantManager.Instance != null) {
            financeManager = RestaurantManager.Instance.FinanceManager;
        } else {
            Debug.LogWarning("RestaurantManager.Instance is null!");
        }
        
        // Auto-assign other managers as needed...
        if(ordersUIManager == null)
            ordersUIManager = FindFirstObjectByType<OrdersUIManager>();
        if(questUIManager == null)
            questUIManager = FindFirstObjectByType<QuestUIManager>();
    }


    #endregion

    #region Desktop Control Methods
    private void HandleTabToggle(bool isDesktopOpen) {
        if(isDesktopOpen)
            ShowDesktop();
        else
            HideDesktop();
    }

    public void ShowDesktop() {
        LinkScripts();
        if(DesktopWindow != null) {
            DesktopWindow.SetActive(true);
            DesktopPannel.SetActive(true);
        } else {
            Debug.LogWarning("DesktopWindow is not assigned!");
        }
        // Call UpdateDesktopBalance to verify event subscription
        UpdateDesktopBalance();
/*        if(questUIManager != null)
            questUIManager.UpdateTasksUI();
        else
            Debug.LogWarning("QuestUIManager is not assigned!");*/

        if(ordersUIManager != null)
            ordersUIManager.UpdateOrdersUI();
        else
            Debug.LogWarning("OrdersUIManager is not assigned!");
    }

    public void HideDesktop() {
        if(DesktopWindow != null) {
            DesktopWindow.SetActive(false);
            DesktopPannel.SetActive(false);
        }
        CloseAllPanels();
    }
    #endregion

    #region Panel Management - Button Functions

    public void BTN_CloseDesktop() {
        // This assumes you have access to the input system; if not, you can also trigger the event.
/*        if( inputAsset != null) {
            inputAsset.CloseDesktop();
        } else {
            EventManager.Trigger("TabToggle", false);
        }*/
    }

    public void BTN_OpenTasksPanel() {
        CloseAllPanels();
        if(TasksPanel != null)
            TasksPanel.SetActive(true);
    }

    public void BTN_CloseTasksPanel() {
        if(TasksPanel != null)
            TasksPanel.SetActive(false);
    }

    public void BTN_OpenOrdersPanel() {
        CloseAllPanels();
        if(OrdersPanel != null)
            OrdersPanel.SetActive(true);
    }

    public void BTN_CloseOrderPanel() {
        if(OrdersPanel != null)
            OrdersPanel.SetActive(false);
    }

    public void BTN_OpenShopPanel() {
        CloseAllPanels();
        if(ShopPanel != null)
            ShopPanel.SetActive(true);
    }

    public void BTN_CloseShopPanel() {
        if(ShopPanel != null)
            ShopPanel.SetActive(false);
    }

    public void BTN_OpenManagementPanel() {
        CloseAllPanels();
        if(ManagementPanel != null)
            ManagementPanel.SetActive(true);
    }

    public void BTN_CloseManagementPanel() {
        if(ManagementPanel != null)
            ManagementPanel.SetActive(false);
    }

    public void BTN_AddPayment() {
        Debug.Log("BTN_AddPayment pressed");
        RestaurantManager.Instance.FinanceManager.AddBusinessIncome(1000, "Payment Received");
    }

    // Finance Panel methods
    public void BTN_OpenFinancePanel() {
        CloseAllPanels();
        if(FinancePanel != null)
            FinancePanel.SetActive(true);
    }

    public void BTN_CloseFinancePanel() {
        if(FinancePanel != null)
            FinancePanel.SetActive(false);
    }

    public void CloseAllPanels() {
        if(TasksPanel != null) TasksPanel.SetActive(false);
        if(OrdersPanel != null) OrdersPanel.SetActive(false);
        if(ShopPanel != null) ShopPanel.SetActive(false);
        if(ManagementPanel != null) ManagementPanel.SetActive(false);
        if(FinancePanel != null) FinancePanel.SetActive(false);
    }
    #endregion

    #region Helper Methods
    void UpdateDesktopBalance() {
        if(financeManager != null) {
            BusinessBalance = financeManager.BusinessBalance;
            if(balanceText != null) {
                foreach(TextMeshProUGUI balanceText in balanceText) {
                    balanceText.text = "$ " + BusinessBalance.ToString();
                }
            } else
                Debug.LogWarning("balanceText is not assigned!");
        } else {
            Debug.LogWarning("FinanceManager is not assigned in UpdateDesktopBalance!");
        }
    }
    #endregion
}
#endregion
