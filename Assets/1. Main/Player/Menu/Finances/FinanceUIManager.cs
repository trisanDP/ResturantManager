using RestaurantManagement;
using TMPro;
using UnityEngine;

public class FinanceUIManager : MonoBehaviour {
    private FinanceManager financeManager;

    // Panels for the Finance App
    public GameObject TransactionHistoryGroup;
    public GameObject BusinessBalanceGroup;
    public TextMeshProUGUI balanceUItxt;

    private void Awake() {
        // Initialize financeManager in Awake so it's available in OnEnable.
        if(RestaurantManager.Instance == null) {
            Debug.LogError("RestaurantManager instance not found!");
        } else {
            financeManager = RestaurantManager.Instance.FinanceManager;
            if(financeManager == null)
                Debug.LogError("Finance Manager not found!");
        }
    }

    private void OnEnable() {
        Debug.Log("Heeeeee");
        // Subscribe to input events.
        EventManager.Subscribe("OnTabPressed", ToggleTabUI);
        // Subscribe to finance events if financeManager is valid.
        if(financeManager != null)
            financeManager.OnBusinessBalanceChanged += UpdateBusinessBalanceUI;

        else
            Debug.LogError("Finance Manager not found in OnEnable!");
    }

    private void OnDisable() {
        // Unsubscribe from input events.
        Debug.Log("Hello");
        EventManager.Unsubscribe("OnTabPressed", ToggleTabUI);
        if(financeManager != null)
            financeManager.OnBusinessBalanceChanged -= UpdateBusinessBalanceUI;
    }

    private void ToggleTabUI(bool isPressed) {
        // Toggle Finance app panels.
        TransactionHistoryGroup.SetActive(isPressed);
        BusinessBalanceGroup.SetActive(isPressed);
    }

    private void UpdateBusinessBalanceUI() {
        Debug.Log("Call");
        if(financeManager != null) {
            decimal amount = financeManager.BusinessBalance;

            balanceUItxt.text = amount.ToString("C");
        } else
            Debug.Log("Empty");
    }
}
