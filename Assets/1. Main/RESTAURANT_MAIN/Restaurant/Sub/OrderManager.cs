using RestaurantManagement;
using UnityEngine;
using System.Collections.Generic;

public class OrderManager {
    #region Fields
    private Queue<Order> pendingOrders = new Queue<Order>();
    private List<Order> completedOrders = new List<Order>(); // Stores completed orders
    #endregion

    #region Order Management Methods
    public void AddOrder(Order order) {
        if(order != null && !pendingOrders.Contains(order)) {
            pendingOrders.Enqueue(order);
            GameNotificationManager.Instance.ShowNotification($"Order Added for: {order.Customer.name}", 3);
        }
    }

    public Order GetNextOrder() {
        return pendingOrders.Count > 0 ? pendingOrders.Dequeue() : null;
    }

    public List<Order> GetPendingOrders() {
        return new List<Order>(pendingOrders);
    }

    public List<Order> GetCompletedOrders() {
        return new List<Order>(completedOrders);
    }

    public void CompleteOrder(Order order) {
        if(order != null && order.Status != Order.OrderStatus.Completed) {
            order.MarkCompleted();
            completedOrders.Add(order);

            // Remove from pending queue
            List<Order> updatedPendingOrders = new List<Order>(pendingOrders);
            updatedPendingOrders.Remove(order);
            pendingOrders = new Queue<Order>(updatedPendingOrders);
        }
    }
    #endregion
}
