using RestaurantManagement.Reservations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestaurantManagement {
    public class Table : MonoBehaviour, IInteractable {

        #region Seat Definition
        [Serializable]
        public class Seat {
            public Transform seatPosition;
            public Transform foodPlacementPoint;
            [HideInInspector] public Customer currentCustomer;
            public bool isReserved;
        }
        #endregion

        #region Fields and Properties
        [Header("Table Settings")]
        [SerializeField] private List<Seat> seats = new List<Seat>();

        private Reservation reservation = new Reservation();
        public event Action<Table> OnSeatStatusChanged;
        public bool HasAvailableSeat => seats.Exists(seat => seat.currentCustomer == null && !seat.isReserved);
        #endregion

        #region Seating Methods
        public bool ReserveSeat(Customer customer) {
            Seat availableSeat = GetAvailableSeat();
            if(availableSeat != null) {
                availableSeat.isReserved = true;
                reservation.Reserve(customer, availableSeat);
                OnSeatStatusChanged?.Invoke(this);
                RestaurantManager.Instance?.TableManager?.NotifyTableAvailabilityChanged(this);
                return true;
            }
            return false;
        }

        private Seat GetAvailableSeat() {
            List<Seat> availableSeats = new List<Seat>();
            foreach(Seat seat in seats) {
                if(seat.currentCustomer == null && !seat.isReserved)
                    availableSeats.Add(seat);
            }
            if(availableSeats.Count == 0)
                return null;
            int randomIndex = UnityEngine.Random.Range(0, availableSeats.Count);
            return availableSeats[randomIndex];
        }

        public void SeatCustomer(Customer customer) {
            // Retrieve the reserved seat from the reservation system.
            Seat reservedSeat = reservation.GetReservedSeat(customer);
            if(reservedSeat != null) {
                reservedSeat.currentCustomer = customer;
                reservedSeat.isReserved = false;
                reservation.ConfirmReservation(customer);

                Vector3 seatPos = reservedSeat.seatPosition.position;
                Vector3 tableCenter = transform.position;
                Vector3 lookDir = (tableCenter - seatPos).normalized;
                Quaternion desiredRotation = (lookDir.sqrMagnitude > 0.001f) ?
                    Quaternion.LookRotation(lookDir) : reservedSeat.seatPosition.rotation;

                // Directly assign seat via the customer's method.

                customer.AssignSeat(seatPos);

                OnSeatStatusChanged?.Invoke(this);
            }
        }

        public Vector3 GetReservedSeatPosition(Customer customer) {
            Seat reservedSeat = reservation.GetReservedSeat(customer);
            if(reservedSeat != null)
                return reservedSeat.seatPosition.position;
            Seat availableSeat = GetAvailableSeat();
            return (availableSeat != null) ? availableSeat.seatPosition.position : transform.position;
        }

        public Seat GetSeatForCustomer(Customer customer) {
            return seats.Find(seat => seat.currentCustomer == customer);
        }

        public void RemoveCustomer(Customer customer) {
            Seat seat = seats.Find(s => s.currentCustomer == customer);
            if(seat != null) {
                seat.currentCustomer = null;
                OnSeatStatusChanged?.Invoke(this);
            }
        }
        #endregion

        #region Food Delivery Method
        // Delivers a cooked food object to the customer who ordered that food.
        public void DeliverFood(FoodObject food) {
            // Iterate over seats and deliver to the matching customer.
            bool delivered = false;
            foreach(var seat in seats) {
                if(seat.currentCustomer != null) {
                    // Compare the food item data of the customer's order to the delivered food.
                    if(seat.currentCustomer.GetOrderedFood() == food.FoodItemData) {
                        // Parent the food object to the seat's food placement point.
                        food.transform.SetParent(seat.foodPlacementPoint);
                        food.transform.localPosition = Vector3.zero;
                        food.transform.localRotation = Quaternion.identity;
                        // Notify the customer of the delivery.
                        seat.currentCustomer.NotifyFoodDelivered(food);
                        delivered = true;
                        break;
                    }
                }
            }
            if(!delivered) {
                Debug.LogWarning("No matching customer found for the delivered food.");
            }
        }
        #endregion

        #region Interaction Methods
        // Handle seating when a customer is selected via the controller.
        private void HandleCustomerSeating(BoxController controller) {
            Customer customer = controller.GetSelectedNPC();
            if(customer != null && HasAvailableSeat && ReserveSeat(customer)) {
                SeatCustomer(customer);
                controller.DeselectNPC();
            }
        }

        // Handle food delivery when a carried box contains cooked food.
        private void HandleFoodDelivery(BoxController controller) {
            FoodObject food = controller.GetCarriedBox()?.GetComponent<FoodObject>();
            if(food != null && food.CurrentCookingState == CookingState.Cooked) {
                DeliverFood(food);
                controller.ClearCarriedBox();
            } else {
                Debug.LogWarning("Food is either not cooked or invalid.");
            }
        }
        #endregion

        #region IInteractable Implementation
        public void OnFocusEnter() {
            // Optional: highlight table.
        }

        public void OnFocusExit() {
            // Optional: remove highlight.
        }

        public void Interact(BoxController controller) {
            // If a customer is selected (NPC), try to seat them.
            if(controller.HasSelectedNPC()) {
                HandleCustomerSeating(controller);
            }
            // Otherwise, if a box is being carried, attempt food delivery.
            else if(controller.HasCarriedBox()) {
                HandleFoodDelivery(controller);
            }
        }
        #endregion

        #region Unity Methods
        private void OnEnable() {
            RestaurantManager.Instance?.TableManager?.RegisterTable(this);
        }

        private void OnDisable() {
            RestaurantManager.Instance?.TableManager?.UnregisterTable(this);
        }

        private void Start() {
            RestaurantManager.Instance?.TableManager?.RegisterTable(this);
        }
        #endregion
    }
}
