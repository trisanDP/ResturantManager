// FoodObject.cs
using UnityEngine;
using static FoodItemData;

public class FoodObject : MonoBehaviour {

    #region Fields and Properties

    [Header("Food Settings")]
    public CookingState CurrentCookingState = CookingState.Raw; // Current cooking state
    public FoodItemData FoodItemData; // Associated food item data
    public string FoodName; // Name of the food

    public float CookingProgress { get; private set; } = 0f; // Cooking progress percentage
    public int CurrentStageIndex { get; private set; } = 0; // Current cooking stage index

    public bool IsCooking => CurrentCookingState == CookingState.Cooking; // Check if the food is being cooked

    #endregion

    #region Cooking Methods

    // Updates cooking progress and adjusts state accordingly
    public void UpdateCookingProgress(float increment) {
        CookingProgress += increment;

        if(CookingProgress > 0 && CookingProgress < 100f) {
            SetCookingState(CookingState.HalfCooked);
        }

        if(CookingProgress >= 100f) {
            CookingProgress = 100f;
            SetCookingState(CookingState.Cooked);
            Debug.Log($"{FoodName} is fully cooked!");
        }
    }

    // Sets the cooking state of the food
    public void SetCookingState(CookingState state) {
        CurrentCookingState = state;
    }

    // Gets the current cooking stage from the food item
    public CookingStage GetCurrentStage() {
        return FoodItemData?.GetCurrentCookingStage(CurrentStageIndex);
    }

    // Advances to the next cooking stage
    public void AdvanceStage() {
        CurrentStageIndex++;
    }

    // Checks if the food is fully cooked
    public bool IsFullyCooked() {
        return CurrentCookingState == CookingState.Cooked;
    }

    #endregion

    #region Unity Methods

    // Ensures the food name matches the associated food item
    private void OnValidate() {
        if(FoodItemData != null) {
            FoodName = FoodItemData.FoodName;
        }
    }

    // Destroys the food object when finished
    public void FinishedEating() {
        Destroy(gameObject);
    }

    #endregion
}
