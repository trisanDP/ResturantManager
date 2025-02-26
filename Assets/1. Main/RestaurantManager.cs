using UnityEngine;
using System.Collections.Generic;

namespace RestaurantManagement {
    public class RestaurantManager : MonoBehaviour {
        public static RestaurantManager Instance;

        [Header("Restaurant Points")]
        public Transform EntrancePoint;
        public Transform ExitPoint;
        public Transform KitchenPosition;
        public Transform WaitPoint; // Waiting point for customers

        [Header("Restaurant Details")]
        public string RestaurantName;

        [Header("Metrics")]
        public float Popularity = 50f;
        public float MarketingLevel = 50f;
        public bool UseSpawnLogic = false;
        public float TotalEarnings { get; private set; }

        [Header("Menu and Tables")]
        public List<FoodItemData> Menu = new List<FoodItemData>();
        public List<Table> Tables = new List<Table>();

        private Queue<Table> pendingOrders = new Queue<Table>();

        private void Awake() {
            if(Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        #region Table Management
        public Table GetAvailableTable() {
            foreach(Table table in Tables) {
                if(table.HasAvailableSeats) {
                    return table;
                }
            }
            return null;
        }
        #endregion

        #region Order Management
        public void AddPendingOrder(Table table) {
            if(table != null && !pendingOrders.Contains(table)) {
                pendingOrders.Enqueue(table);
                Debug.Log($"Added pending order for table: {table.name}");
            }
        }

        public Table GetNextPendingOrder() {
            return pendingOrders.Count > 0 ? pendingOrders.Dequeue() : null;
        }
        #endregion

        #region Payment and Metrics
        public void ProcessPayment(float amount) {
            TotalEarnings += amount;
            Debug.Log($"Restaurant earned ${amount}. Total: ${TotalEarnings}");
        }

        public void UpdateMetrics(float popularityChange, float marketingChange) {
            Popularity = Mathf.Clamp(Popularity + popularityChange, 0, 100);
            MarketingLevel = Mathf.Clamp(MarketingLevel + marketingChange, 0, 100);
            Debug.Log($"Updated metrics: Popularity = {Popularity}, Marketing = {MarketingLevel}");
        }
        #endregion

        #region Menu Management
        public void AddDishToMenu(FoodItemData dish) {
            if(dish != null && !Menu.Contains(dish)) {
                Menu.Add(dish);
                Debug.Log($"Added {dish.FoodName} to the menu.");
            }
        }

        public void RemoveDishFromMenu(FoodItemData dish) {
            if(dish != null && Menu.Contains(dish)) {
                Menu.Remove(dish);
                Debug.Log($"Removed {dish.FoodName} from the menu.");
            }
        }
        #endregion
    }
}
