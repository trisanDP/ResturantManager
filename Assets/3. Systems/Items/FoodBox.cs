
// FoodObject.cs
using UnityEngine;

public class FoodObject : MonoBehaviour {

    #region Fields and Properties

    [Header("Food Settings")]
    public CookingState CurrentCookingState = CookingState.Raw;
    public FoodItem FoodItem;
    public string FoodName;
    public float RemainingCookingTime { get; private set; } = 0f;
    public int CurrentStageIndex { get; private set; } = 0;

    [Range(0,100)]
    public float CookingProgress = 0f;
    public float MaxProgress;

    #endregion

    #region Cooking Methods

    public void UpdateCookingProgress(float increment) {
        CookingProgress += increment;
        if(CookingProgress >= MaxProgress) {
            CookingProgress = MaxProgress;
            SetCookingState(CookingState.Cooked);
            Debug.Log($"{FoodName} is fully cooked!");
        }
    }

    public void SetCookingState(CookingState state) {
        CurrentCookingState = state;
    }

    public void SetCookingTime(float remainingTime) {
        RemainingCookingTime = remainingTime;
    }

    public bool IsFullyCooked() {
        if(CurrentCookingState == CookingState.Cooked)
            return true;
        else
            return false;
/*        return CurrentStageIndex >= FoodItem.CookingStages.Length;*/
    }

    public void AdvanceCookingStage() {
        RemainingCookingTime = 0f; // Reset cooking time
        CurrentStageIndex++;

        if(IsFullyCooked()) {
            Debug.Log($"{FoodName} is fully cooked and ready to serve!");
            SetCookingState(CookingState.Cooked);
        } else {
            Debug.Log($"{FoodName} is ready for the next stage.");
            SetCookingState(CookingState.Cooking);
        }
    }

    #endregion

    #region Unity Methods

    private void OnValidate() {
        if(FoodItem != null) {
            FoodName = FoodItem.FoodName;
        }
    }

    #endregion

    public void FinishedEating() {
        Destroy(gameObject);
    }
}
