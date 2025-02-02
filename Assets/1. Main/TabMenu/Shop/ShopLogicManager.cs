using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLogicManager : MonoBehaviour {
    #region Fields and Properties

    [Header("Item Database")]
    public ItemDatabase itemDatabase; // Reference to the item database
    private Dictionary<FoodItemData, int> cartItems = new(); // Cart items and quantities

    public IReadOnlyDictionary<FoodItemData, int> CartItems => cartItems; // Read-only access

    private FinanceManager financeManager;

    #endregion

    private void Awake() {
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
    }
    private void Start() {

        financeManager = GameManager.Instance.FinanceManager; // Get the finance manager directly
    }
    #region Cart Management

    public void AddToCart(FoodItemData item, int quantity = 1) {
        if(cartItems.ContainsKey(item)) {
            cartItems[item] += quantity;
        } else {
            cartItems[item] = quantity;
        }
        Debug.Log($"Added {item.FoodName} x{quantity} to cart.");
    }

    public void RemoveFromCart(FoodItemData item) {
        if(cartItems.ContainsKey(item)) {
            cartItems.Remove(item);
            Debug.Log($"Removed {item.FoodName} from cart.");
        }
    }

    public void ClearCart() {
        cartItems.Clear();
        Debug.Log("Cart cleared.");
    }

    public decimal GetTotalCartCost() {
        decimal totalCost = 0;
        foreach(var cartItem in cartItems) {
            totalCost += (decimal)cartItem.Key.cost * cartItem.Value; // Convert item cost to decimal
        }
        return totalCost;
    }

    public bool CanAffordCart() {
        return financeManager.GetBusinessBalance() >= GetTotalCartCost(); // Compare using decimal
    }

    public void DeductCartCost() {
        decimal totalCost = GetTotalCartCost();
        financeManager.AddExpenditure(totalCost, "Purchased cart items"); // Use decimal total cost
    }


    #endregion
}
