using UnityEngine;

public class FinanceUIManager : MonoBehaviour {

    [SerializeField] private FinanceManager financeManager;

    private void Awake() {
        financeManager = GameManager.Instance.FinanceManager;
    }

    private void OnEnable() {
        // Subscribe to input events
        EventManager.Subscribe("OnTabPressed", ToggleTabUI);

        // Subscribe to finance events
        financeManager.OnPersonalBalanceChanged.AddListener(UpdatePersonalBalanceUI);
        financeManager.OnBusinessBalanceChanged.AddListener(UpdateBusinessBalanceUI);
    }

    private void OnDisable() {
        // Unsubscribe from input events
        EventManager.Unsubscribe("OnTabPressed", ToggleTabUI);

        // Unsubscribe from finance events
        financeManager.OnPersonalBalanceChanged.RemoveListener(UpdatePersonalBalanceUI);
        financeManager.OnBusinessBalanceChanged.RemoveListener(UpdateBusinessBalanceUI);
    }

    private void ToggleTabUI(bool isPressed) {
        // Toggle Tab UI
    }

    private void UpdatePersonalBalanceUI() {
        // Update personal balance UI
    }

    private void UpdateBusinessBalanceUI() {
        // Update business balance UI
    }
}
