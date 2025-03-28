// CookingStation.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region CookingStation Class
public abstract class CookingStation : MonoBehaviour, IInteractable {
    #region Fields & Properties
    [Header("Cooking Table Settings")]
    public Transform[] CookingSpots;
    public CookingStationType TableType;  // This station's type (e.g., Oven, PrepTable, Stove)
    public int Force;

    private readonly List<CookingSlot> _cookingSlots = new();
    private readonly Dictionary<FoodObject, Coroutine> _activeCookingCoroutines = new();
    protected bool _isCooking;
    #endregion

    #region Unity Methods
    private void Awake() {
        // Create a cooking slot for each cooking spot.
        foreach(var spot in CookingSpots) {
            var slot = new CookingSlot(spot);
            _cookingSlots.Add(slot);
        }
    }

    private void Start() {
        CookingStationManager.Instance.Register(this);
    }
    #endregion

    #region Interaction Methods
    public virtual void OnFocusEnter() {
        // Optionally highlight this station.
    }

    public virtual void OnFocusExit() {
        // Optionally remove highlight.
    }

    public void Interact(BoxController controller) {
        var carriedBox = controller.GetCarriedBox();
        var foodObject = carriedBox?.GetComponent<FoodObject>();

        if(foodObject != null && carriedBox.CurrentInteractionState == InteractionState.Carried) {
            TryStartCooking(foodObject, controller);
        } else {
            Debug.Log("No food box is being carried or invalid state.");
        }
    }
    #endregion

    #region Cooking Management
    protected virtual void TryStartCooking(FoodObject foodObject, BoxController controller) {
        if(foodObject == null || foodObject.FoodItemData == null) {
            Debug.LogWarning("Invalid food box or food item!");
            return;
        }

        var currentStage = foodObject.GetCurrentStage();
        // Check if the food's current cooking stage is compatible with this station.
        if(currentStage == null || currentStage.RequiredTableType != TableType) {
            GameNotificationManager.Instance.ShowNotification("Invalid cooking table for this food item.", 1f);
            return;
        }

        var availableSlot = GetAvailableSlot();
        if(availableSlot == null) {
            Debug.LogWarning("No available cooking spots.");
            return;
        }

        StartCookingProcess(foodObject, controller, availableSlot);
    }

    private CookingSlot GetAvailableSlot() {
        return _cookingSlots.Find(slot => slot.IsAvailable);
    }

    private void StartCookingProcess(FoodObject foodObject, BoxController controller, CookingSlot slot) {
        var boxObject = foodObject.GetComponent<BoxObject>();
        boxObject.Attach(slot.Spot, Vector3.zero, Quaternion.identity);
        boxObject.SetInteractionState(InteractionState.CookingPlaced);

        foodObject.SetCookingState(CookingState.Cooking);
        slot.AssignFoodBox(foodObject);
        controller.ClearCarriedBox();

        Coroutine cookingCoroutine = StartCoroutine(ProcessFood(slot));
        _activeCookingCoroutines[foodObject] = cookingCoroutine;
    }

    protected IEnumerator ProcessFood(CookingSlot slot) {
        var foodObject = slot.FoodBox;
        var stage = foodObject.GetCurrentStage();

        if(stage == null)
            yield break;

        float stageProgressIncrement = 100f / foodObject.FoodItemData.CookingStages.Length;
        float duration = stage.Duration;
        float elapsedTime = 0f;

        while(elapsedTime < duration && foodObject.CookingProgress < 100f) {
            yield return null;

            if(slot == null)
                break;

            float deltaTime = Time.deltaTime;
            elapsedTime += deltaTime;
            slot.UpdateTime(deltaTime);

            float progressIncrement = (deltaTime / duration) * stageProgressIncrement;
            foodObject.UpdateCookingProgress(progressIncrement);
            _isCooking = true;
        }

        _isCooking = false;
        var rb = foodObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * Force, ForceMode.Impulse);

        if(foodObject.CookingProgress < 100f) {
            foodObject.AdvanceStage();
        }

        slot.Clear();
    }
    #endregion

    #region Suitability Check
    // Check if this station is suitable for the given food item based on its current cooking stage.
    public bool IsSuitableFor(FoodItemData data) {
        if(data == null || data.CookingStages == null || data.CookingStages.Length == 0)
            return false;
        // Here we assume stage 0 is the current stage; adjust if you maintain a dynamic stage index.
        var stage = data.CookingStages[0];
        return stage.RequiredTableType == TableType;
    }
    #endregion
}
#endregion

#region CookingSlot Class
public class CookingSlot {
    public Transform Spot { get; }
    public FoodObject FoodBox { get; private set; }
    public float RemainingTime { get; private set; }

    public bool IsAvailable => FoodBox == null;

    public CookingSlot(Transform spot) {
        Spot = spot;
        Clear();
    }

    public void AssignFoodBox(FoodObject foodBox) {
        FoodBox = foodBox;
        RemainingTime = foodBox.GetCurrentStage()?.Duration ?? 0f;
    }

    public void Clear() {
        FoodBox = null;
        RemainingTime = 0f;
    }

    public void UpdateTime(float deltaTime) {
        if(RemainingTime > 0)
            RemainingTime -= deltaTime;
    }
}
#endregion

public enum CookingStationType {
    PrepTable,
    Stove,
    Oven
}
