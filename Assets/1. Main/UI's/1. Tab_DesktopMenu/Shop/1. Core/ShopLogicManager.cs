using RestaurantManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLogicManager : MonoBehaviour {
    #region Fields and Properties

    [Header("Item Database")]
    public ItemDatabase itemDatabase; // Reference to the item database

    // Cart items and quantities
    private Dictionary<FoodItemData, int> cartItems = new();
    public IReadOnlyDictionary<FoodItemData, int> CartItems => cartItems;

    // Event to notify UI updates whenever the cart is modified
    public event Action OnCartUpdated = delegate { };

    private FinanceManager financeManager;

    #endregion

    private void Awake() {
        // Load the database (assuming it's in Resources folder)
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        if(itemDatabase == null) {
            Debug.LogError("ItemDatabase not found in Resources folder.");
        }
    }
    private void Start() {
        // Get the finance manager from your central RestaurantManager singleton
        financeManager = RestaurantManager.Instance.FinanceManager;
    }

    #region Cart Management

    // Adds an item to the cart (or increments quantity if it exists)
    public void AddToCart(FoodItemData item, int quantity = 1) {
        if(cartItems.ContainsKey(item)) {
            cartItems[item] += quantity;
        } else {
            cartItems[item] = quantity;
        }
        Debug.Log($"Added {item.FoodName} x{quantity} to cart.");
        OnCartUpdated?.Invoke();
    }

    // Updates the quantity of an item in the cart (delta can be positive or negative)
    public void UpdateCartItemQuantity(FoodItemData item, int delta) {
        if(cartItems.ContainsKey(item)) {
            cartItems[item] += delta;
            if(cartItems[item] <= 0) {
                cartItems.Remove(item);
            }
            OnCartUpdated?.Invoke();
        }
    }

    // Remove an item entirely from the cart.
    public void RemoveFromCart(FoodItemData item) {
        if(cartItems.ContainsKey(item)) {
            cartItems.Remove(item);
            Debug.Log($"Removed {item.FoodName} from cart.");
            OnCartUpdated?.Invoke();
        }
    }

    public void ClearCart() {
        cartItems.Clear();
        Debug.Log("Cart cleared.");
        OnCartUpdated?.Invoke();
    }

    // Calculates the total cost of the cart.
    public decimal GetTotalCartCost() {
        decimal totalCost = 0;
        foreach(var cartItem in cartItems) {
            totalCost += (decimal)cartItem.Key.costPrice * cartItem.Value;
        }
        return totalCost;
    }

    public bool CanAffordCart() {
        return financeManager.BusinessBalance >= GetTotalCartCost();
    }

    public void DeductCartCost() {
        decimal totalCost = GetTotalCartCost();
        financeManager.DeductBusinessExpense(totalCost, "Purchased cart items");
    }

    // When purchasing, check funds, deduct expense, and spawn items.
    public void PurchaseCart() {
        if(!CanAffordCart()) {
            Debug.Log("Too expensive. Cannot complete purchase.");
            return;
        }

        DeductCartCost();
        StartCoroutine(SpawnCartItems());
    }

    // Spawns each cart item at a designated location (using a prefab reference from RestaurantManager)
    private IEnumerator SpawnCartItems() {
        // Create a copy of the dictionary to iterate over.
        foreach(var cartItem in new Dictionary<FoodItemData, int>(cartItems)) {
            FoodItemData item = cartItem.Key;
            int quantity = cartItem.Value;
            for(int i = 0; i < quantity; i++) {
                yield return new WaitForSeconds(0.5f); // Delay between spawns
                GameObject go = Instantiate(RestaurantManager.Instance.FoodBoxPrefab,
                                            RestaurantManager.Instance.ShopSpawnPoint.position,
                                            RestaurantManager.Instance.ShopSpawnPoint.rotation);
                go.GetComponent<FoodObject>().FoodItemData = item;
                Debug.Log($"{item.FoodName} has been spawned!");
            }
        }
        ClearCart();
    }

    #endregion
}
