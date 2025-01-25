using UnityEngine;

public class TabMenuManager : MonoBehaviour
{
    public FinanceManager financeManager;

    public void Awake() {
        financeManager = FindFirstObjectByType<FinanceManager>();
    }

}
