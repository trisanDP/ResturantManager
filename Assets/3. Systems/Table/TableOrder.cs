using System.Collections.Generic;

namespace RestaurantManagement {
    public class TableOrder {
        // Maps each customer to their ordered food
        // Maps each customer to their ordered food
        public Dictionary<Customer, FoodItemData> CustomerOrders { get; } = new Dictionary<Customer, FoodItemData>();
        // Adds an order for a specific customer
        public void AddOrder(Customer customer, FoodItemData food) {
            if(customer == null || food == null) return;
            CustomerOrders[customer] = food;
        }

        // Gets the food ordered by a specific customer
        public FoodItemData GetFoodForCustomer(Customer customer) {
            return CustomerOrders.ContainsKey(customer) ? CustomerOrders[customer] : null;
        }

        // Gets the customer who ordered a specific food (for food delivery)
        public Customer GetCustomerForFood(FoodItemData food) {
            foreach(var entry in CustomerOrders) {
                if(entry.Value == food) return entry.Key;
            }
            return null;
        }
    }
}