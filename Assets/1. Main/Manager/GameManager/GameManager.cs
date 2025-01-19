using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public FinanceManager FinanceManager;

    private void Awake() {
        if(Instance == null)
            Instance = this; 
        else
            Destroy(Instance);
        Instantiate();
    }

    void Instantiate() {
        LinkedScripts();
    }

    void LinkedScripts() {
        if(FinanceManager == null) {
            FinanceManager = FindFirstObjectByType<FinanceManager>();
            GameUIMessage.Instance.DisplayMessage("Consuming Perfromance",2);
        }

    }
}
