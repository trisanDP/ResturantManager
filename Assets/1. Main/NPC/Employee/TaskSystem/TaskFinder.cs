// TaskFinder.cs
#region TaskFinder
using RestaurantManagement;
using System.Collections.Generic;
using UnityEngine;

public static class TaskFinder {
    // Returns a task for a given employee role if available.
    public static EmployeeTask GetAvailableTask(EmployeeRole role) {
        if(role == EmployeeRole.Cook) {
            // Example: fetch a pending order from the OrderManager.
            var order = RestaurantManager.Instance.OrderManager.GetNextOrder();
            if(order != null) {
                // Example: find a shelf with raw items for this food.
                var shelf = ShelfManager.Instance.GetShelfFor(order.FoodItemData);
                // Example: find an available station for that food item.
                var station = EnvironmentManager.Instance.GetStationFor(order.FoodItemData);
                if(shelf != null && station != null) {
                    return new CookingTask {
                        OrderData = order,
                        TargetShelf = shelf,
                        TargetStation = station
                    };
                }
            }
        }
        return null;
    }
}
#endregion
