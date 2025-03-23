using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RestaurantManagement;

public class MenuItemUI : MonoBehaviour {
    #region Fields
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;    // Displays the food name.
    public TextMeshProUGUI priceText;   // Displays price (to customer).
    public TextMeshProUGUI profitText;  // Displays profit per sale.
    public Button addButton;            // Button to add the item.
    public Button removeButton;         // Button to remove the item.

    private FoodItemData foodItem;
    private MenuBookManager menuBook;
    #endregion

    #region Initialization
    public void Initialize(FoodItemData item, MenuBookManager manager) {
        foodItem = item;
        menuBook = manager;

        // Set text values.
        nameText.text = foodItem.FoodName;
        // For demonstration, assume price is 2x cost and profit equals cost.
        priceText.text = $"Price: ${foodItem.sellPrice}";
        profitText.text = $"Profit: ${foodItem.sellPrice - foodItem.costPrice}";

        // Connect button actions.
        addButton.onClick.AddListener(() => menuBook.AddItemToMenu(foodItem));
        removeButton.onClick.AddListener(() => menuBook.RemoveItemFromMenu(foodItem));
    }
    #endregion
}
