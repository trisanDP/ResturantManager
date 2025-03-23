using TMPro;
using UnityEngine;
using static FoodItemData;

public class FoodObject : MonoBehaviour {
    [Header("Food Settings")]
    public CookingState CurrentCookingState = CookingState.Raw;
    public FoodItemData FoodItemData;
    public string FoodName;
    public TextMeshProUGUI FoodNameTxt;

    public float CookingProgress { get; private set; } = 0f;
    public int CurrentStageIndex { get; private set; } = 0;

    public bool IsCooking => CurrentCookingState == CookingState.Cooking;

    private void Start() {
        if(FoodItemData != null) {
            FoodName = FoodItemData.FoodName;
            FoodNameTxt.text = FoodName;
        } else {
            Debug.LogError("FoodItemData is null");
        }
    }

    // Updates cooking progress and adjusts the state accordingly.
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

    // Sets the current cooking state.
    public void SetCookingState(CookingState state) {
        CurrentCookingState = state;
    }

    // Returns the current cooking stage.
    public CookingStage GetCurrentStage() {
        return FoodItemData?.GetCurrentCookingStage(CurrentStageIndex);
    }

    // Advances to the next cooking stage.
    public void AdvanceStage() {
        CurrentStageIndex++;
    }

    // Checks if the food is fully cooked.
    public bool IsFullyCooked() {
        return CurrentCookingState == CookingState.Cooked;
    }

    // Ensure that the food name always matches the associated food item.
    private void OnValidate() {
        if(FoodItemData != null) {
            FoodName = FoodItemData.FoodName;
        }
    }

    // Destroys the food object when finished.
    public void FinishedEating() {
        Destroy(gameObject);
    }
}
