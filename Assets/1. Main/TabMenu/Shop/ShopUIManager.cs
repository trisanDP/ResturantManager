using RestaurantManagement;
using UnityEngine;

public class ShopUIManager : MonoBehaviour {
    #region Fields and Properties

    [Header("UI Elements")]
    public Transform shopContent; // Parent for UI elements
    public GameObject ShopUiTemplete; // Prefab for UI elements

    private ShopLogicManager shopLogicManager;

    public GameObject ShopUiGroup;

    public float VerticalOffset=100f;
    public float HorizontalOffset=100f;

    #endregion

    private void Start() {
        shopLogicManager = FindFirstObjectByType<ShopLogicManager>(); // Directly get the ShopLogicManager
        PopulateShopUI();
        Hide();
    }

    #region UI Management

    private void PopulateShopUI() {
     
        int index = 0;

        foreach(var item in shopLogicManager.itemDatabase.activeItems) {
            var foodBox = Instantiate(ShopUiTemplete, shopContent);
            var ui = foodBox.GetComponent<ItemUITemplete>();
            ui.Initialize(item, shopLogicManager);

            // Position adjustment
            var rectTransform = foodBox.GetComponent<RectTransform>();
            if(rectTransform != null) {
                rectTransform.anchoredPosition = new Vector2(HorizontalOffset * -index,VerticalOffset * -index );
            }
/*
            ui.addToCartButton.onClick.AddListener(() => shopLogicManager.AddToCart(item));*/
            index++;
        }
    }

    public void Show() {
        ShopUiGroup.SetActive(true);
    }

    public void Hide() {
        ShopUiGroup.SetActive(false);
    }

    #endregion
}
