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

    // Method to order a food item
    public void OrderFood(FoodItemData foodItem) {
        if(shopLogicManager.TryPurchase(foodItem)) {
            Debug.Log($"Ordered {foodItem.FoodName} for {foodItem.cost}.");
            StartCoroutine(ProcessOrder(foodItem));
        } else {
            Debug.LogWarning($"Not enough balance to order {foodItem.FoodName}!");
        }
    }

    // Coroutine to process the order and spawn the food object
    private IEnumerator ProcessOrder(FoodItemData foodItem) {
        Debug.Log($"Preparing {foodItem.FoodName}...");
        yield return new WaitForSeconds(orderDelay); // Simulate order delay

        // Now instantiate the food item in ShopManager after purchase
        GameObject go = Instantiate(FoodBoxObject, spawnPoint.position, spawnPoint.rotation);
        go.GetComponent<FoodObject>().FoodItemData = foodItem;
        Debug.Log($"{foodItem.FoodName} has arrived!");
    }

    #endregion
}
