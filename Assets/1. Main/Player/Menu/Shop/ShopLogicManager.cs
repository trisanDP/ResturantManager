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

    #endregion

    #region Purchase Logic

    public bool TryPurchase(FoodItemData item) {
        if(financeManager.GetBusinessBalance() >= item.cost) {
            financeManager.AddExpenditure(item.cost, $"Purchased {item.FoodName}");
            return true;
        }
        return false;
    }

    public IEnumerator PurchaseItems(float delay) {
        foreach(var cartItem in cartItems) {
            var item = cartItem.Key;
            var quantity = cartItem.Value;

            for(int i = 0; i < quantity; i++) {
                if(TryPurchase(item)) {
                    yield return new WaitForSeconds(delay);
                } else {
                    Debug.LogError("Insufficient funds for purchase.");
                    yield break;
                }
            }
        }

        ClearCart();
    }

    #endregion
}
