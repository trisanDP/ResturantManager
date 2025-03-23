using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartUIManager : MonoBehaviour {
    [Header("Cart UI Elements")]
    public Transform cartContent;         // Parent container for cart items
    public GameObject cartItemPrefab;       // Prefab for a cart item (see next section)
    public Button buyNowButton;             // Button to trigger purchase
    public TextMeshProUGUI totalCostText;   // Displays the total cost

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

    // Clears and repopulates the cart UI based on current items.
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
    }

    private void OnBuyNowClicked() {
        shopLogicManager.PurchaseCart();
    }
}
