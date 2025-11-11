using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RestaurantManagement;

public class CartUIManager : MonoBehaviour {
    [Header("Cart UI Elements")]         
    public Transform cartContent;        
    public GameObject cartItemPrefab;     
    public Button buyNowButton;          
    public TextMeshProUGUI totalCostText;

    public TextMeshProUGUI txt_Balance;

    private ShopLogicManager shopLogicManager;
    private void Awake() {
        shopLogicManager = FindFirstObjectByType<ShopLogicManager>();
        if(shopLogicManager == null) {
            Debug.LogError("ShopLogicManager not found!");
        }
    }


    private void Start() {
        shopLogicManager.OnCartUpdated += RefreshCartUI;
        buyNowButton.onClick.AddListener(OnBuyNowClicked);
        RefreshCartUI();
    }

    private void OnDisable() {
        shopLogicManager.OnCartUpdated -= RefreshCartUI;
    }

    private void RefreshCartUI() {
        // Clear previous cart UI entries.
        foreach(Transform child in cartContent) {
            Destroy(child.gameObject);
        }
        // Instantiate a UI element for each cart item.
        foreach(var kvp in shopLogicManager.CartItems) {
            FoodItemData item = kvp.Key;
            int quantity = kvp.Value;
            GameObject cartItemGO = Instantiate(cartItemPrefab, cartContent);
            CartItemUI cartItemUI = cartItemGO.GetComponent<CartItemUI>();
            cartItemUI.Initialize(item, quantity, shopLogicManager);
        }
        totalCostText.text = $"Total: ${shopLogicManager.GetTotalCartCost()}";
        txt_Balance.text = RestaurantManager.Instance.FinanceManager.BusinessBalance.ToString();
    }

    private void OnBuyNowClicked() {
        shopLogicManager.PurchaseCart();
    }
}
