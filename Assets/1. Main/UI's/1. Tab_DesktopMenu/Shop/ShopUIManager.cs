using RestaurantManagement;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour {
    #region Fields and Properties

    [Header("UI Elements")]
    public Transform shopContent;      // Parent for UI elements
    public GameObject ShopUITemplate;  // Prefab for UI elements
    public float VerticalOffset = 100f;
    public float HorizontalOffset = 100f;


    [Header("Cart")]
    public GameObject CartPannel;


    private ShopLogicManager shopLogicManager;
    private ShopProgressManager shopProgressManager;

    #endregion

    private void Start() {
        BTN_CloseCart();
        shopLogicManager = FindFirstObjectByType<ShopLogicManager>();
        shopProgressManager = FindFirstObjectByType<ShopProgressManager>();

        // Load progress using the item database from shopLogicManager.
        shopProgressManager.LoadProgress(shopLogicManager.itemDatabase);

        PopulateShopUI();
    }

    private void PopulateShopUI() {
        int index = 0;
        // Loop through all items and display only those that are unlocked.
        foreach(var item in shopLogicManager.itemDatabase.allItems) {
            if(shopProgressManager.IsItemUnlocked(item.FoodID)) {
                GameObject foodBox = Instantiate(ShopUITemplate, shopContent);
                ItemUITemplete ui = foodBox.GetComponent<ItemUITemplete>();
                ui.Initialize(item, shopLogicManager);

                // Adjust position if needed.
                RectTransform rectTransform = foodBox.GetComponent<RectTransform>();
                if(rectTransform != null) {
                    rectTransform.anchoredPosition = new Vector2(HorizontalOffset * -index, VerticalOffset * -index);
                }
                index++;
            }
        }
    }

    public void BTN_OpenCart() {
        CartPannel.SetActive(true);
    }

    public void BTN_CloseCart() {
        CartPannel.SetActive(false);
    }
}
