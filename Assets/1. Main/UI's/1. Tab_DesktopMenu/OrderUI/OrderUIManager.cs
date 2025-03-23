using UnityEngine;
using TMPro;
using System.Collections.Generic;
using RestaurantManagement;

public class OrdersUIManager : MonoBehaviour {
    #region Variables
    public static OrdersUIManager Instance;
    public GameObject orderItemPrefab; // Prefab for a single order item
    public Transform pendingOrderListParent; // Parent for pending orders
    public Transform completedOrderListParent; // Parent for completed orders

    private List<Order> pendingOrders = new List<Order>();
    private List<Order> completedOrders = new List<Order>();
    #endregion

    #region Unity Methods
    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region UI Update Methods
    public void UpdateOrdersUI() {
        ClearOrderList(pendingOrderListParent);
        ClearOrderList(completedOrderListParent);

        // Fetch orders from OrderManager
        pendingOrders = RestaurantManager.Instance.OrderManager.GetPendingOrders();
        completedOrders = RestaurantManager.Instance.OrderManager.GetCompletedOrders();

        // Display pending orders
        foreach(Order order in pendingOrders) {
            GameObject orderItem = Instantiate(orderItemPrefab, pendingOrderListParent);
            TextMeshProUGUI textComp = orderItem.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = order.Customer != null
                ? $"{order.FoodItemData.name} to {order.Customer.name} in {order.Table.name}"
                : $"{order.FoodItemData.name} (Customer Left! Order Complete)";
        }

        // Display completed orders
        foreach(Order order in completedOrders) {
            GameObject orderItem = Instantiate(orderItemPrefab, completedOrderListParent);
            TextMeshProUGUI textComp = orderItem.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = order.Customer != null
                ? $" {order.FoodItemData.name} (Served to {order.Customer.name})"
                : $" {order.FoodItemData.name} (Order Completed)";
        }
    }

    private void ClearOrderList(Transform parent) {
        foreach(Transform child in parent)
            Destroy(child.gameObject);
    }
    #endregion
}
