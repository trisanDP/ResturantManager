using UnityEngine;

namespace RestaurantManagement {
    // Helper to handle order placement and food delivery.
    public class CustomerOrderFoodHandler {
        private Customer customer;
        public CustomerOrderFoodHandler(Customer customer) {
            this.customer = customer;
        }

        public bool TryPlaceOrder() {
            if(customer.GetOrderedFood() == null) {
                FoodItemData dish = ChooseDishFromMenu();
                if(dish != null) {
                    customer.SetOrderedFood(dish);
                    Table table = customer.AssignedTable;
                    if(table != null) {
                        var seat = table.GetSeatForCustomer(customer);
                        if(seat != null) {
                            Order newOrder = new Order(customer, table, seat, dish);
                            RestaurantManager.Instance.OrderManager.AddOrder(newOrder);
                            customer.SetActiveOrder(newOrder);  // NEW: Link order to customer
                            return true;
                        }
                        Debug.LogWarning("No seat found for order placement.");
                        return false;
                    }
                    Debug.LogWarning("Customer has no assigned table for ordering.");
                    return false;
                }
                Debug.LogWarning("Menu is empty or no dish available.");
                return false;
            }
            return true;
        }


        public void NotifyFoodDelivered(FoodObject food) {
            if(IsValidFood(food)) {
                customer.SetCurrentFood(food);
                // Store the food data for payment later
                customer.SetServedFoodData(food.FoodItemData);
                float eatDuration = CalculateEatingDuration(food);
                customer.StartEating(eatDuration);
                // Mark the associated order as served
                Order order = customer.GetActiveOrder();
                if(order != null) {
                    order.MarkServed();
                }
            }
        }


        public void TryDeliverFood(BoxController controller) {
            FoodObject food = controller.GetCarriedBox()?.GetComponent<FoodObject>();
            if(IsValidFood(food)) {
                controller.ClearCarriedBox();
                NotifyFoodDelivered(food);
            }
        }

        #region Private Helpers
        private bool IsValidFood(FoodObject food) {
            return food != null &&
                   food.CurrentCookingState == CookingState.Cooked &&
                   food.FoodItemData == customer.GetOrderedFood();
        }

        private float CalculateEatingDuration(FoodObject food) {
            float qualityMultiplier = customer.GetQualityMultiplier(food.FoodItemData.CurrentQuality);
            return 5f / (customer.eatingSpeed * qualityMultiplier);
        }

        private FoodItemData ChooseDishFromMenu() {
            var menu = RestaurantManager.Instance?.MenuManager?.MenuItems;
            if(menu == null || menu.Count == 0) {
                GameNotificationManager.Instance.ShowNotification("Menu is empty!", 5);
                return null;
            }
            return menu[UnityEngine.Random.Range(0, menu.Count)];
        }
        #endregion
    }
}
