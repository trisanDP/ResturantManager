using System.Collections;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    #region Fields and Properties

    [Header("Order Settings")]
    public Transform spawnPoint; // Where the food items will spawn
    public float orderDelay = 5f; // Time it takes for the order to arrive

    private ShopLogicManager shopLogicManager;
    public GameObject FoodBoxObject;

    #endregion

    private void Start() {
        shopLogicManager = GetComponent<ShopLogicManager>(); // Directly get the ShopLogicManager
    }

    #region Order Methods

    public void AddToCart(FoodItemData foodItem) {
        shopLogicManager.AddToCart(foodItem);
    }

    public void PurchaseCart() {
        if(!shopLogicManager.CanAffordCart()) {
            Debug.Log("Too expensive. Cannot complete purchase.");
            return;
        }

        shopLogicManager.DeductCartCost(); // Deduct the total cost
        StartCoroutine(SpawnCartItems());
    }

    private IEnumerator SpawnCartItems() {
        foreach(var cartItem in shopLogicManager.CartItems) {
            var item = cartItem.Key;
            var quantity = cartItem.Value;

            for(int i = 0; i < quantity; i++) {
                yield return new WaitForSeconds(orderDelay);

                // Instantiate the item
                GameObject go = Instantiate(FoodBoxObject, spawnPoint.position, spawnPoint.rotation);
                go.GetComponent<FoodObject>().FoodItemData = item;
                Debug.Log($"{item.FoodName} has arrived!");
            }
        }

        shopLogicManager.ClearCart(); // Clear the cart after purchase
    }

    #endregion
}
