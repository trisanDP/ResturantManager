using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookingUIManager : MonoBehaviour {
    /*[SerializeField] private Transform uiContainer; // Parent transform for UI elements
    [SerializeField] private GameObject cookingItemPrefab; // Prefab for a single UI element

    private Dictionary<CookingProcess, GameObject> uiElements = new Dictionary<CookingProcess, GameObject>();

    private void OnEnable() {
        CookingManager.Instance.OnCookingUpdated += UpdateUI;
    }

    private void OnDisable() {
        CookingManager.Instance.OnCookingUpdated -= UpdateUI;
    }

    private void UpdateUI() {
        var activeProcesses = CookingManager.Instance.GetActiveProcesses();

        // Remove UI for completed processes
        foreach(var process in new List<CookingProcess>(uiElements.Keys)) {
            if(!activeProcesses.Contains(process)) {
                Destroy(uiElements[process]);
                uiElements.Remove(process);
            }
        }

        // Add or update UI for active processes
        foreach(var process in activeProcesses) {
            if(!uiElements.ContainsKey(process)) {
                GameObject uiElement = Instantiate(cookingItemPrefab, uiContainer);
                uiElements[process] = uiElement;
            }
            UpdateUIElement(uiElements[process], process);
        }
    }

    private void UpdateUIElement(GameObject uiElement, CookingProcess process) {
        // Assuming UI prefab has components like Text or TMP_Text
        var foodNameText = uiElement.transform.Find("FoodName").GetComponent<TMPro.TextMeshProUGUI>();
        var tableNameText = uiElement.transform.Find("TableName").GetComponent<TMPro.TextMeshProUGUI>();
        var timeRemainingText = uiElement.transform.Find("TimeRemaining").GetComponent<TMPro.TextMeshProUGUI>();

        foodNameText.text = process.FoodBox.FoodName;
        tableNameText.text = process.CookingStation.name;
        timeRemainingText.text = $"{process.TimeRemaining:F1} seconds";
    }*/
}
