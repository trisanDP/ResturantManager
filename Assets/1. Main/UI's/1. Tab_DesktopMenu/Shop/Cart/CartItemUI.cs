using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartItemUI : MonoBehaviour {
    public TextMeshProUGUI foodNameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;
    public Button plusButton;
    public Button minusButton;
    public Image foodImage; // Optional: set this if you add an image field to FoodItemData.

    private FoodItemData itemData;
    private ShopLogicManager shopLogicManager;
    private int quantity;

    // Initialize with the cart item's data and the current quantity.
    public void Initialize(FoodItemData itemData, int initialQuantity, ShopLogicManager shopLogicManager) {
        this.itemData = itemData;
        this.shopLogicManager = shopLogicManager;
        quantity = initialQuantity;
        UpdateUI();

        plusButton.onClick.AddListener(OnPlusClicked);
        minusButton.onClick.AddListener(OnMinusClicked);
    }

    private void UpdateUI() {
        foodNameText.text = itemData.FoodName;
        priceText.text = $"${itemData.sellPrice}";
        quantityText.text = quantity.ToString();
        // If you have an image reference in FoodItemData, you can set it here.
        // e.g., foodImage.sprite = itemData.foodSprite;
    }

    // Increase quantity.
    private void OnPlusClicked() {
        quantity++;
        shopLogicManager.UpdateCartItemQuantity(itemData, 1);
        UpdateUI();
    }

    // Decrease quantity or remove item if quantity reaches zero.
    private void OnMinusClicked() {
        if(quantity > 1) {
            quantity--;
            shopLogicManager.UpdateCartItemQuantity(itemData, -1);
        } else {
            // Remove the item entirely.
            shopLogicManager.RemoveFromCart(itemData);
        }
        UpdateUI();
    }
}
