using UnityEngine;
using System.Collections.Generic;

namespace RestaurantManagement {
    public class Table : MonoBehaviour, IInteractable {
        #region Fields and Properties
        [Header("Settings")]
        [SerializeField] private Transform[] seatingPositions;
        [SerializeField] private Transform[] foodPlacementPoints;

        private Customer[] seatedCustomers;
        private bool[] seatReserved;
        private Dictionary<Customer, int> reservedSeats = new Dictionary<Customer, int>();

        private TableOrder orders = new TableOrder();

        public bool HasAvailableSeats => GetAvailableSeatIndex() != -1;
        #endregion

        #region Initialization
        private void Start() {
            seatedCustomers = new Customer[seatingPositions.Length];
            seatReserved = new bool[seatingPositions.Length];
        }
        #endregion

        #region Seating Management
        private int GetAvailableSeatIndex() {
            for(int i = 0; i < seatingPositions.Length; i++) {
                if(seatedCustomers[i] == null && !seatReserved[i]) {
                    return i;
                }
            }
            return -1;
        }

        // Reserves a seat for the given customer.
        public bool Reserve(Customer customer) {
            int index = GetAvailableSeatIndex();
            if(index != -1) {
                seatReserved[index] = true;
                reservedSeats[customer] = index;
                return true;
            }
            return false;
        }

        // Returns the reserved chair position for the customer.
        public Vector3 GetReservedChairPosition(Customer customer) {
            if(reservedSeats.TryGetValue(customer, out int index)) {
                return seatingPositions[index].position;
            }
            // Fallback: return any available seat position.
            int availableIndex = GetAvailableSeatIndex();
            return availableIndex != -1 ? seatingPositions[availableIndex].position : transform.position;
        }

        public void SeatCustomer(Customer customer) {
            if(reservedSeats.TryGetValue(customer, out int index)) {
                seatedCustomers[index] = customer;
                seatReserved[index] = false; // Reservation fulfilled
                reservedSeats.Remove(customer);
                customer.AssignSeat(seatingPositions[index].position, seatingPositions[index].rotation);
            }
        }

        public void RemoveCustomer(Customer customer) {
            int seatIndex = System.Array.IndexOf(seatedCustomers, customer);
            if(seatIndex != -1) {
                seatedCustomers[seatIndex] = null; // Free the seat
            }
        }
        #endregion

        #region Order Management
        public void TakeOrder(Customer customer, FoodItemData food) {
            orders.AddOrder(customer, food);
            RestaurantManager.Instance.AddPendingOrder(this);
        }

        public void DeliverFood(FoodObject food) {
            Customer targetCustomer = orders.GetCustomerForFood(food.FoodItemData);
            if(targetCustomer != null) {
                PlaceFoodAtTable(food, targetCustomer);
                targetCustomer.NotifyFoodDelivered(food);
            }
        }

        private void PlaceFoodAtTable(FoodObject food, Customer customer) {
            int seatIndex = System.Array.IndexOf(seatedCustomers, customer);
            if(seatIndex != -1 && seatIndex < foodPlacementPoints.Length) {
                food.transform.SetParent(foodPlacementPoints[seatIndex]);
                food.transform.localPosition = Vector3.zero;
                food.transform.localRotation = Quaternion.identity;
            }
        }
        #endregion

        #region IInteractable Implementation
        public void OnFocusEnter() {
            // Optionally highlight table
        }
        public void OnFocusExit() {
            // Remove highlight
        }
        public void Interact(BoxController controller) {
            if(controller.HasSelectedNPC()) {
                HandleCustomerSeating(controller);
            } else if(controller.HasCarriedBox()) {
                HandleFoodDelivery(controller);
            }
        }
        private void HandleCustomerSeating(BoxController controller) {
            Customer customer = controller.GetSelectedNPC();
            if(HasAvailableSeats && Reserve(customer)) {
                SeatCustomer(customer);
                controller.DeselectNPC();
            }
        }
        private void HandleFoodDelivery(BoxController controller) {
            FoodObject food = controller.GetCarriedBox()?.GetComponent<FoodObject>();
            if(food != null && food.CurrentCookingState == CookingState.Cooked) {
                DeliverFood(food);
                controller.ClearCarriedBox();
            }
        }
        #endregion
    }
}
