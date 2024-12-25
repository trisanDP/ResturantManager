using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("NPC Settings")]
    public float EatingSpeed = 1f; // Speed multiplier for eating food

    private FoodObject _currentFood;
    private bool _isEating = false;
    private Table _assignedTable;

    #endregion

    #region Eating Logic

    // Assigns a table to the NPC
    public void AssignTable(Table table) {
        _assignedTable = table;
        CheckForFoodOnTable();
    }

    // Starts the eating process for the given food box
    public void StartEating(FoodObject food) {
        if(_isEating) {
            Debug.LogWarning($"{name} is already eating.");
            return;
        }

        if(food == null || food.CurrentCookingState != CookingState.Cooked) {
            Debug.LogWarning($"{name} cannot eat: Food is invalid or not cooked.");
            return;
        }

        _currentFood = food;
        _isEating = true;

        Debug.Log($"{name} started eating {_currentFood.FoodName}.");
        StartCoroutine(EatFood());
    }

    // Handles the eating process over time
    private IEnumerator EatFood() {
        if(_currentFood == null || _currentFood.FoodItem == null) {
            Debug.LogWarning($"{name} cannot eat: Missing food or food data.");
            yield break;
        }

        // Calculate eating time based on NPC speed and food quality
        float baseEatTime = 5f; // Default time to eat any food
        float qualityMultiplier = GetQualityMultiplier(_currentFood.FoodItem.CurrentQuality);
        float eatTime = baseEatTime / (EatingSpeed * qualityMultiplier);

        yield return new WaitForSeconds(eatTime);

        Debug.Log($"{name} finished eating {_currentFood.FoodName}.");
        _currentFood.FinishedEating();
        _currentFood = null;
        _isEating = false;

        // Check for more food on the table after finishing
        CheckForFoodOnTable();
    }

    // Returns a multiplier based on food quality
    private float GetQualityMultiplier(FoodItem.FoodQuality quality) {
        return quality switch {
            FoodItem.FoodQuality.Low => 0.75f,
            FoodItem.FoodQuality.Mid => 1f,
            FoodItem.FoodQuality.High => 1.25f,
            _ => 1f
        };
    }

    // Checks for available food on the assigned table
    public void CheckForFoodOnTable() {
        if(_assignedTable == null) {
            Debug.LogWarning($"{name} has no assigned table to check for food.");
            return;
        }

        FoodObject food = _assignedTable.GetAvailableFood();
        if(food != null) {
            StartEating(food);
        } else {
            Debug.Log($"{name} found no food on the table.");
        }
    }

    #endregion

    #region Teleport Logic

    // Instantly moves the NPC to the given position and rotation
    public void TeleportTo(Vector3 position, Quaternion rotation) {
        transform.SetPositionAndRotation(position, rotation);
        Debug.Log($"{name} teleported to the table.");
    }

    #endregion

    #region IInteractable Implementation

    public void OnFocusEnter() {
        Debug.Log($"Player is focusing on NPC {name}.");
    }

    public void OnFocusExit() {
        Debug.Log($"Player stopped focusing on NPC {name}.");
    }

    public void Interact(BoxController controller) {
        if(controller.HasSelectedNPC()) {
            Debug.LogWarning($"Another NPC is already selected: {controller.GetSelectedNPC().name}");
            return;
        }

        controller.SelectNPC(this);
        Debug.Log($"{name} has been selected by the player.");
    }

    #endregion
}
