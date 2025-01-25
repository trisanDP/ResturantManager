using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("NPC Settings")]
    public FoodItemData OrderedFood; // The food this NPC has ordered
    public float EatingSpeed = 1f; // Speed multiplier for eating food
    public GameObject SelectedIdentifier;

    private FoodObject _currentFood;
    private bool _isEating = false;
    private Table _assignedTable;

    public bool HasOrderedFood => OrderedFood != null; // Check if the NPC has an active order

    #endregion

    #region Eating Logic
    void Start() {
        SelectedIdentifier.SetActive(false);
    }

    public void AssignTable(Table table) {
        _assignedTable = table;
        OrderFood(OrderedFood);
    }

    public void StartEating(FoodObject food) {
        if(_isEating) {
            Debug.LogWarning($"{name} is already eating.");
            return;
        }

        if(food == null || food.CurrentCookingState != CookingState.Cooked) {
            Debug.LogWarning($"{name} cannot eat: Food is invalid or not cooked.");
            return;
        }

        if(OrderedFood == null || food.FoodItemData != OrderedFood) {
            Debug.LogWarning($"{name} did not order this food.");
            return;
        }

        _currentFood = food;
        _isEating = true;
        Debug.Log($"{name} started eating {_currentFood.FoodName}.");
        StartCoroutine(EatFood());
    }

    private IEnumerator EatFood() {
        if(_currentFood == null || _currentFood.FoodItemData == null) {
            Debug.LogWarning($"{name} cannot eat: Missing food or food data.");
            yield break;
        }

        float baseEatTime = 5f;
        float qualityMultiplier = GetQualityMultiplier(_currentFood.FoodItemData.CurrentQuality);
        float eatTime = baseEatTime / (EatingSpeed * qualityMultiplier);

        yield return new WaitForSeconds(eatTime);

        Debug.Log($"{name} finished eating {_currentFood.FoodName}.");
        _currentFood.FinishedEating();
        _currentFood = null;
        _isEating = false;
        OrderedFood = null; // Clear the order after eating
    }

    private float GetQualityMultiplier(FoodItemData.FoodQuality quality) {
        return quality switch {
            FoodItemData.FoodQuality.Low => 0.75f,
            FoodItemData.FoodQuality.Mid => 1f,
            FoodItemData.FoodQuality.High => 1.25f,
            _ => 1f
        };
    }

    public void OrderFood(FoodItemData foodItem) {
        if(_assignedTable == null) {
            Debug.LogWarning($"{name} is not assigned to a table.");
            return;
        }

        _assignedTable.TakeOrder(this, foodItem);
    }

    public void NotifyFoodPlaced(FoodObject food) {
        StartEating(food);
    }

    #endregion

    #region Seating Logic

    public void TeleportTo(Vector3 position, Quaternion rotation) {
        transform.SetPositionAndRotation(position, rotation);
        Debug.Log($"{name} seated at a new chair.");
    }

    #endregion

    #region IInteractable Implementation

    public void OnFocusEnter() {
        // Debug.Log($"Player is focusing on NPC {name}.");
    }

    public void OnFocusExit() {
        // Debug.Log($"Player stopped focusing on NPC {name}.");
    }

    public void Interact(BoxController controller) {
        if(controller.HasCarriedBox()) {
            var foodBox = controller.GetCarriedBox()?.GetComponent<FoodObject>();
            if(foodBox != null) {
                StartEating(foodBox);
                controller.ClearCarriedBox();
            }
        } else {
            controller.SelectNPC(this);
        }
    }

    #endregion
}
