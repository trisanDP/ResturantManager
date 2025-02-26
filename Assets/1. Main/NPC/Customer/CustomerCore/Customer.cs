using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace RestaurantManagement {
    public class Customer : MonoBehaviour, IInteractable, IMovable {
        #region Fields and Properties
        [Header("Settings")]
        [SerializeField] private float eatingSpeed = 1f;
        [SerializeField] private float orderDelay = 1f;    // Delay to simulate ordering
        [SerializeField] private float paymentDelay = 1f;  // Delay to simulate payment
        [SerializeField] private GameObject selectedIndicator;
        [SerializeField] private float navMeshStoppingDistance = 0.3f; // Increased stopping distance

        [Header("Debug")]
        [SerializeField] private TextMeshProUGUI stateDebugText;

        private FoodItemData orderedFood;
        private Table assignedTable;
        private FoodObject currentFood;
        private NavMeshAgent agent;

        // A timer used for delays in states like ordering, eating, and paying.
        private float stateTimer = 0f;

        public enum State {
            MovingToWaitingPoint,
            WaitingForTable,
            MovingToTable,
            Ordering,
            WaitingForFood,
            Eating,
            Paying,
            Leaving
        }
        private State currentState;
        #endregion

        #region Unity Methods
        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = navMeshStoppingDistance;
            agent.autoBraking = false; // Disable auto-braking to reduce jitter
        }

        private void Start() {
            currentState = State.MovingToWaitingPoint;
            if(RestaurantManager.Instance.WaitPoint != null) {
                MoveTo(RestaurantManager.Instance.WaitPoint.position);
            } else {
                Debug.LogError("RestaurantManager.WaitPoint not assigned!");
            }
        }

        private void Update() {
            UpdateState();
            UpdateDebugText();
        }
        #endregion

        #region State Management
        private void UpdateState() {
            switch(currentState) {
                case State.MovingToWaitingPoint:
                if(HasReachedDestination()) {
                    currentState = State.WaitingForTable;
                }
                break;

                case State.WaitingForTable:
                if(FindAndReserveTable()) {
                    MoveTo(assignedTable.GetReservedChairPosition(this));
                    currentState = State.MovingToTable;
                }
                break;

                case State.MovingToTable:
                if(HasReachedDestination()) {
                    assignedTable.SeatCustomer(this);
                    // Start ordering: set a delay timer.
                    stateTimer = orderDelay;
                    currentState = State.Ordering;
                }
                break;

                case State.Ordering:
                // Countdown the ordering delay.
                stateTimer -= Time.deltaTime;
                if(stateTimer <= 0f) {
                    PlaceOrder();
                    // PlaceOrder() will set the state to WaitingForFood if successful.
                }
                break;

                case State.WaitingForFood:
                // In this state, we wait for an external call to NotifyFoodDelivered.
                break;

                case State.Eating:
                // Countdown the eating duration.
                stateTimer -= Time.deltaTime;
                if(stateTimer <= 0f) {
                    currentState = State.Paying;
                    stateTimer = paymentDelay; // Set delay for payment.
                }
                break;

                case State.Paying:
                // Countdown the payment delay.
                stateTimer -= Time.deltaTime;
                if(stateTimer <= 0f) {
                    ProcessPayment();
                    LeaveRestaurant();
                }
                break;

                case State.Leaving:
                if(HasReachedDestination()) {
                    Destroy(gameObject);
                }
                break;
            }
        }
        #endregion

        #region Navigation and Movement
        public bool IsMoving => agent.velocity.sqrMagnitude > 0.01f;

        public void MoveTo(Vector3 destination) {
            // Ensure the agent is enabled and not stopped.
            if(!agent.enabled)
                agent.enabled = true;
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public void StopMovement() {
            if(agent != null)
                agent.ResetPath();
        }

        public bool HasReachedDestination() {
            // Added an extra threshold (0.1f) to account for precision errors.
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f;
        }
        #endregion

        #region Core Behavior
        private bool FindAndReserveTable() {
            if(assignedTable == null) {
                assignedTable = RestaurantManager.Instance.GetAvailableTable();
                if(assignedTable != null && assignedTable.Reserve(this)) {
                    return true;
                }
                assignedTable = null;
            }
            return false;
        }

        private void PlaceOrder() {
            if(orderedFood == null) {
                orderedFood = ChooseDishFromMenu();
                if(orderedFood != null) {
                    assignedTable.TakeOrder(this, orderedFood);
                    currentState = State.WaitingForFood;
                } else {
                    Debug.LogWarning("No dish available in the menu.");
                    LeaveRestaurant();
                }
            }
        }

        // This method is called externally (e.g., from the Table) when food is delivered.
        public void NotifyFoodDelivered(FoodObject food) {
            if(IsValidFood(food)) {
                currentFood = food;
                // Calculate eating duration based on food quality and eating speed.
                float eatDuration = CalculateEatingDuration();
                stateTimer = eatDuration;
                currentState = State.Eating;
            }
        }

        private void LeaveRestaurant() {
            if(assignedTable != null) {
                assignedTable.RemoveCustomer(this);
            }
            currentState = State.Leaving;
            // Re-enable the agent before leaving
            if(!agent.enabled)
                agent.enabled = true;

            if(SpawnManager.Instance != null && SpawnManager.Instance.exitLocation != null) {
                MoveTo(SpawnManager.Instance.exitLocation.position);
            } else if(RestaurantManager.Instance.ExitPoint != null) {
                MoveTo(RestaurantManager.Instance.ExitPoint.position);
            } else {
                Debug.LogError("No exit location defined!");
            }
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

        private void ProcessPayment() {
            float payment = currentFood.FoodItemData.cost * GetQualityMultiplier(currentFood.FoodItemData.CurrentQuality);
            RestaurantManager.Instance.ProcessPayment(payment);
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
                stateDebugText.text = currentState.ToString();
        }
        #endregion

        #region Additional Methods
        // Called by Table to assign the seat position to the customer.
        public void AssignSeat(Vector3 seatPosition, Quaternion seatRotation) {
            StopMovement();
            // Disable the agent when the customer is seated to prevent further adjustments.
            if(agent != null)
                agent.enabled = false;
            transform.position = seatPosition;
            transform.rotation = seatRotation;
        }
        #endregion

        #region IInteractable Implementation
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
