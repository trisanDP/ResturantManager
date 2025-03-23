using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using RestaurantManagement;

public class MenuSetterUI : MonoBehaviour {
    #region Fields
    [Header("UI Elements")]
    public Transform itemListContainer;  // Parent container for unlocked items (left side).
    public Transform activeMenuContainer; // Parent container for active menu items (right side).
    public GameObject menuItemPrefab;    // Prefab for unlocked item UI (add/remove buttons).
    public GameObject menuNamePrefab;    // Prefab for displaying names of active menu items.
    public Button applyButton;
    public Button goToStoreButton;
    #endregion

    #region Unity Methods
    private void Start() {
        // Get reference to the MenuBookManager
        MenuBookManager menuBook = RestaurantManager.Instance.MenuManager;

        // Subscribe to menu update events
        menuBook.OnMenuUpdated += RefreshUI;

        applyButton.onClick.AddListener(ApplyChanges);
        goToStoreButton.onClick.AddListener(OpenStore);

        RefreshUI();
    }

    private void OnDestroy() {
        if(RestaurantManager.Instance?.MenuManager != null)
            RestaurantManager.Instance.MenuManager.OnMenuUpdated -= RefreshUI;
    }
    #endregion

    #region UI Update Methods
    private void RefreshUI() {
        RefreshUnlockedItems();
        RefreshActiveMenu();
    }

    // Populates unlocked items with Add/Remove buttons
    private void RefreshUnlockedItems() {
        foreach(Transform child in itemListContainer) {
            Destroy(child.gameObject);
        }

        List<FoodItemData> unlockedItems = RestaurantManager.Instance.MenuManager.GetUnlockedItems();
        foreach(FoodItemData item in unlockedItems) {
            GameObject obj = Instantiate(menuItemPrefab, itemListContainer);

            MenuItemUI itemUI = obj.GetComponent<MenuItemUI>();
            itemUI.Initialize(item, RestaurantManager.Instance.MenuManager);
        }
    }

    // Populates the list of active menu items (only names)
    private void RefreshActiveMenu() {
        foreach(Transform child in activeMenuContainer) {
            Destroy(child.gameObject);

        }

        List<FoodItemData> activeItems = RestaurantManager.Instance.MenuManager.MenuItems;
        foreach(FoodItemData item in activeItems) {
            GameObject go = Instantiate(menuNamePrefab, activeMenuContainer);
            TextMeshProUGUI textComp = go.GetComponentInChildren<TextMeshProUGUI>();
            if(textComp != null) {
                textComp.text = item.FoodName;
            }
        }
    }
    #endregion

    #region Button Callbacks
    private void ApplyChanges() {
        Debug.Log("Menu changes applied.");
        RestaurantManager.Instance.MenuManager.SaveState();
    }

    private void OpenStore() {
        ShopLogicManager shopLogic = FindFirstObjectByType<ShopLogicManager>();
        foreach(FoodItemData item in RestaurantManager.Instance.MenuManager.MenuItems) {
            shopLogic.AddToCart(item);
        }
        Debug.Log("Selected menu items added to cart.");
    }
    #endregion
}
