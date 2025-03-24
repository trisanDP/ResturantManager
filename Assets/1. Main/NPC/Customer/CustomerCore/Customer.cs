using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

namespace RestaurantManagement {
    public class Customer : MonoBehaviour, IInteractable, IMovable {
        #region Fields and Properties
        [Header("Settings")]
        public float eatingSpeed = 1f;
        public float orderDelay = 1f;
        public float paymentDelay = 1f;
        [SerializeField] private GameObject selectedIndicator;
        [SerializeField] private float navMeshStoppingDistance = 0.3f;

        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI stateDebugText;

        // Core Components
        public Animator animator;
        private NavMeshAgent agent;
        private Rigidbody rb;
        private CustomerMovementController movementController;
        private CustomerStateMachine stateMachine;

        // Helpers
        private CustomerOrderFoodHandler orderFoodHandler;
        private CustomerSeatingHandler seatingHandler;

        // Data
        private FoodItemData orderedFood;
        private FoodObject currentFood;
        private FoodItemData servedFoodData;   // NEW: Stores food info after delivery for payment
        private Order activeOrder;             // NEW: Stores the active order
        private Table assignedTable;
        public Table AssignedTable => assignedTable;
        #endregion

        #region Unity Callbacks
        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            movementController = new CustomerMovementController(agent, navMeshStoppingDistance);
            stateMachine = new CustomerStateMachine();
            stateMachine.Initialize(this, movementController.HasReachedDestination, movementController.MoveTo);

            orderFoodHandler = new CustomerOrderFoodHandler(this);
            seatingHandler = new CustomerSeatingHandler(this);

            RestaurantManager.Instance?.CustomerManager?.RegisterCustomer(this);
        }

        private void Start() {
            if(RestaurantManager.Instance?.WaitPoint != null)
                movementController.MoveTo(RestaurantManager.Instance.WaitPoint.position);
            else
                Debug.LogError("WaitPoint not assigned!");
            CustomerUIManager.Instance?.UpdateCustomerUI(this);
        }

        private void Update() {
            stateMachine.Update(Time.deltaTime);
            UpdateDebugText();

            if(animator != null) {
                bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
                animator.SetBool("IsWalking", isWalking);
            }
        }

        private void OnDestroy() {
            RestaurantManager.Instance?.CustomerManager?.UnregisterCustomer(this);
        }
        #endregion

        #region Interface Implementations
        public void MoveTo(Vector3 position) {
            movementController.MoveTo(position);
        }

        public void StopMovement() {
            movementController.StopMovement();
        }

        public bool IsMoving => movementController.IsMoving;

        public void OnFocusEnter() {
            selectedIndicator?.SetActive(true);
        }

        public void OnFocusExit() {
            selectedIndicator?.SetActive(false);
        }

        public void Interact(BoxController controller) {
            // If carrying a box (e.g., cooked food), delegate to orderFoodHandler.
            if(controller.HasCarriedBox())
                orderFoodHandler.TryDeliverFood(controller);
        }
        #endregion

        #region Seating & Table Handling
        public Table FindAvailableTable() {
            if(assignedTable == null && RestaurantManager.Instance?.TableManager != null) {
                Table table = RestaurantManager.Instance.TableManager.GetAvailableTable();
                if(table != null && seatingHandler.ReserveTable(table))
                    return table;
            }
            Debug.LogWarning("No available table found.");
            return null;
        }

        public void SetAssignedTable(Table table) {
            assignedTable = table;
        }

        public bool IsTableAvailable() {
            return RestaurantManager.Instance?.TableManager?.HasAvailableTable() ?? false;
        }

        public void AssignSeatAtTable() {
            if(assignedTable != null) {
                assignedTable.SeatCustomer(this);
                FreezeMovementAndRotation();
            } else {
                Debug.LogWarning("No assigned table for seating.");
            }
        }

        // Called by Table.SeatCustomer to update the customer's position.
        public void AssignSeat(Vector3 seatPosition/*, Quaternion seatRotation*/) {
            FreezeMovementAndRotation();
            transform.position = seatPosition;
            transform.rotation = Quaternion.Euler(0, 260, 0);

        }
        #endregion

        #region Order & Food Handling
        public void FinishEatingFood() {
            if(currentFood != null) {
                currentFood.FinishedEating(); // Should destroy the FoodObject
                currentFood = null;
            }
            // When finished eating, complete the active order if it exists
            if(activeOrder != null) {
                RestaurantManager.Instance.OrderManager.CompleteOrder(activeOrder);
                Debug.Log("Call 1");
                activeOrder = null;
            }
        }

        public bool PlaceOrder() {
            return orderFoodHandler.TryPlaceOrder();
        }

        public void NotifyFoodDelivered(FoodObject food) {
            orderFoodHandler.NotifyFoodDelivered(food);
        }
        #endregion

        #region Payment & Leaving
        public void ProcessPayment() {
            // Use servedFoodData (stored at delivery) for payment calculation
            FoodItemData foodData = GetServedFoodData();
            if(foodData != null) {
                float payment = foodData.sellPrice * GetQualityMultiplier(foodData.CurrentQuality);
                RestaurantManager.Instance?.FinanceManager?.AddBusinessIncome((decimal)payment, "Customer Payment");
                SetServedFoodData(null);
            } else {
                Debug.LogWarning("No served food data available for payment.");
            }
        }

        public void LeaveRestaurant() {
            seatingHandler.LeaveRestaurant();
        }

        public void DestroySelf() {
            Destroy(gameObject);
        }
        #endregion

        #region Movement Helpers
        public void FreezeMovementAndRotation() {
            if(agent != null) agent.enabled = false;
            if(rb != null) rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        public void UnfreezeMovementAndRotation() {
            if(rb != null) rb.constraints = RigidbodyConstraints.None;
        }
        #endregion

        #region Helper Accessors
        public FoodItemData GetOrderedFood() {
            return orderedFood;
        }

        public void SetOrderedFood(FoodItemData food) {
            orderedFood = food;
        }

        public void SetCurrentFood(FoodObject food) {
            currentFood = food;
        }

        public FoodObject GetCurrentFood() {
            return currentFood;
        }

        // NEW: Served Food Data accessors (for payment after eating)
        public void SetServedFoodData(FoodItemData data) {
            servedFoodData = data;
        }

        public FoodItemData GetServedFoodData() {
            return servedFoodData;
        }

        // NEW: Active Order Management
        public void SetActiveOrder(Order order) {
            activeOrder = order;
        }

        public Order GetActiveOrder() {
            return activeOrder;
        }

        public void StartEating(float duration) {
            stateMachine.StartEating(duration);
        }

        public float GetQualityMultiplier(FoodItemData.FoodQuality quality) {
            switch(quality) {
                case FoodItemData.FoodQuality.Low: return 0.75f;
                case FoodItemData.FoodQuality.Mid: return 1f;
                case FoodItemData.FoodQuality.High: return 1.25f;
                default: return 1f;
            }
        }
        #endregion

        #region Utility Methods
        private void UpdateDebugText() {
            if(stateDebugText != null)
                stateDebugText.text = stateMachine.CurrentState.ToString();
        }
        #endregion
    }
}
