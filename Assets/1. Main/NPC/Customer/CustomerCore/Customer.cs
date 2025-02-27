using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace RestaurantManagement {
    public class Customer : MonoBehaviour, IInteractable, IMovable {
        #region Fields and Properties
        [Header("Settings")]
        public float eatingSpeed = 1f;
        public float orderDelay = 1f;    // Delay to simulate ordering
        public float paymentDelay = 1f;  // Delay to simulate payment
        [SerializeField] private GameObject selectedIndicator;
        [SerializeField] private float navMeshStoppingDistance = 0.3f;

        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI stateDebugText;

        private FoodItemData orderedFood;
        private Table assignedTable;
        private FoodObject currentFood;
        private NavMeshAgent agent;

        // Helper controllers for movement and state handling.
        private CustomerMovementController movementController;
        private CustomerStateMachine stateMachine;

        public Table AssignedTable => assignedTable;
        #endregion

        #region Unity Methods
        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
            movementController = new CustomerMovementController(agent, navMeshStoppingDistance);
            stateMachine = new CustomerStateMachine();
            // Initialize state machine with movement delegates.
            stateMachine.Initialize(this, movementController.HasReachedDestination, movementController.MoveTo);
        }

        private void Start() {
            if(RestaurantManager.Instance.WaitPoint != null) {
                movementController.MoveTo(RestaurantManager.Instance.WaitPoint.position);
            } else {
                Debug.LogError("RestaurantManager.WaitPoint not assigned!");
            }
        }

        private void Update() {
            stateMachine.Update(Time.deltaTime);
            UpdateDebugText();
        }
        #endregion

        #region IMovable Interface Implementation
        public void MoveTo(Vector3 position) {
            movementController.MoveTo(position);
        }

        public void StopMovement() {
            movementController.StopMovement();
        }

        public bool IsMoving => movementController.IsMoving;
        #endregion

        #region Business Logic Methods (Called by State Machine)
        public bool FindAndReserveTable() {
            if(assignedTable == null) {
                assignedTable = RestaurantManager.Instance.GetAvailableTable();
                if(assignedTable != null && assignedTable.Reserve(this)) {
                    return true;
                }
                assignedTable = null;
            }
            return false;
        }

        public bool TryPlaceOrder() {
            if(orderedFood == null) {
                orderedFood = ChooseDishFromMenu();
                if(orderedFood != null) {
                    assignedTable.TakeOrder(this, orderedFood);
                    return true;
                } else {
                    Debug.LogWarning("No dish available in the menu.");
                    return false;
                }
            }
            return true;
        }

        public void AssignSeat(Vector3 seatPosition, Quaternion seatRotation) {
            StopMovement();
            // Optionally disable the NavMeshAgent if no further movement is expected.
            if(agent != null)
                agent.enabled = false;
            transform.position = seatPosition;
            transform.rotation = seatRotation;
        }


        public void AssignSeatAtTable() {
            assignedTable.SeatCustomer(this);
            // Disable the agent to prevent further movement adjustments once seated.
            if(agent != null)
                agent.enabled = false;
        }

        public void ProcessPayment() {
            float payment = currentFood.FoodItemData.cost * GetQualityMultiplier(currentFood.FoodItemData.CurrentQuality);
            RestaurantManager.Instance.ProcessPayment(payment);
        }

        public void LeaveRestaurant() {
            if(assignedTable != null) {
                assignedTable.RemoveCustomer(this);
            }
            // Re-enable the agent so that the customer can navigate away.
            if(!agent.enabled)
                agent.enabled = true;

            if(SpawnManager.Instance != null && SpawnManager.Instance.exitLocation != null) {
                movementController.MoveTo(SpawnManager.Instance.exitLocation.position);
            } else if(RestaurantManager.Instance.ExitPoint != null) {
                movementController.MoveTo(RestaurantManager.Instance.ExitPoint.position);
            } else {
                Debug.LogError("No exit location defined!");
            }
        }

        public void DestroySelf() {
            Destroy(gameObject);
        }
        #endregion

        #region Utility Methods
        private bool IsValidFood(FoodObject food) {
            return food != null &&
                   food.CurrentCookingState == CookingState.Cooked &&
                   food.FoodItemData == orderedFood;
        }

        private float CalculateEatingDuration() {
            float qualityMultiplier = GetQualityMultiplier(currentFood.FoodItemData.CurrentQuality);
            return 5f / (eatingSpeed * qualityMultiplier);
        }

        private float GetQualityMultiplier(FoodItemData.FoodQuality quality) {
            return quality switch {
                FoodItemData.FoodQuality.Low => 0.75f,
                FoodItemData.FoodQuality.Mid => 1f,
                FoodItemData.FoodQuality.High => 1.25f,
                _ => 1f
            };
        }

        private FoodItemData ChooseDishFromMenu() {
            if(RestaurantManager.Instance.Menu.Count == 0) {
                GameNotificationManager.Instance.ShowNotification("Menu is empty!", 5);
                return null;
            }
            int randomIndex = Random.Range(0, RestaurantManager.Instance.Menu.Count);
            return RestaurantManager.Instance.Menu[randomIndex];
        }

        private void UpdateDebugText() {
            if(stateDebugText)
                stateDebugText.text = stateMachine.CurrentState.ToString();
        }
        #endregion

        #region External Notifications and IInteractable Implementation
        // Called externally (e.g., from the Table) when food is delivered.
        public void NotifyFoodDelivered(FoodObject food) {
            if(IsValidFood(food)) {
                currentFood = food;
                float eatDuration = CalculateEatingDuration();
                stateMachine.StartEating(eatDuration);
            }
        }

        public void OnFocusEnter() {
            selectedIndicator.SetActive(true);
        }

        public void OnFocusExit() {
            selectedIndicator.SetActive(false);
        }

        public void Interact(BoxController controller) {
            if(controller.HasCarriedBox()) {
                TryDeliverFood(controller);
            }
        }

        private void TryDeliverFood(BoxController controller) {
            FoodObject food = controller.GetCarriedBox()?.GetComponent<FoodObject>();
            if(IsValidFood(food)) {
                controller.ClearCarriedBox();
                NotifyFoodDelivered(food);
            }
        }
        #endregion
    }
}
