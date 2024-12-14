using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodItem;

public abstract class CookingTable : MonoBehaviour, IInteractable {
    [Header("Cooking Table Settings")]
    public Transform[] CookingSpots;
    public CookingTableType TableType;

    private readonly List<CookingSlot> _cookingSlots = new();
    private readonly Dictionary<FoodBoxObject, Coroutine> _activeCookingCoroutines = new();

    #region Unity Methods
    private void Awake() {
        foreach(var spot in CookingSpots) {
            _cookingSlots.Add(new CookingSlot(spot));
        }
    }
    #endregion

    #region Interaction Methods
    public void OnFocusEnter() {
        Debug.Log($"Focusing on {name}");
    }

    public void OnFocusExit() {
        Debug.Log($"Stopped focusing on {name}");
    }

    public void Interact(BoxController controller) {
        FoodBoxObject carriedBox = controller.GetCarriedBox();

        if(carriedBox != null && carriedBox.CurrentState == ItemState.Carried) {
            TryStartCooking(carriedBox, controller);
        } else {
            Debug.Log("No food box is being carried or invalid state.");
        }
    }
    #endregion

    #region Cooking Management
    protected virtual void TryStartCooking(FoodBoxObject foodBox, BoxController controller) {
        if(foodBox == null || foodBox.FoodItem == null) {
            Debug.LogWarning("Invalid food box or food item!");
            return;
        }

        var currentStage = foodBox.FoodItem.GetCurrentCookingStage(foodBox.CurrentStageIndex);

        if(currentStage == null || currentStage.RequiredTableType != TableType) {
            Debug.LogWarning($"This table cannot handle {foodBox.FoodName} at the current stage.");
            return;
        }

        var availableSlot = GetAvailableSlot(foodBox);
        if(availableSlot == null) {
            Debug.LogWarning("No available cooking spots.");
            return;
        }

        float cookingTime = foodBox.RemainingCookingTime > 0 ? foodBox.RemainingCookingTime : currentStage.Duration;
        PlaceFoodBoxOnTable(foodBox, controller, availableSlot, cookingTime);
    }

    private void PlaceFoodBoxOnTable(FoodBoxObject foodBox, BoxController controller, CookingSlot slot, float duration) {
        foodBox.Attach(slot.Spot, Vector3.zero, Quaternion.identity);
        foodBox.SetState(ItemState.Cooking);
        slot.AssignFoodBox(foodBox, duration);
        controller.ClearCarriedBox();

        Coroutine cookingCoroutine = StartCoroutine(ProcessFood(slot));
        _activeCookingCoroutines[foodBox] = cookingCoroutine;
    }

    public void RemoveFoodBox(FoodBoxObject foodBox) {
        var slot = _cookingSlots.Find(s => s.FoodBox == foodBox);

        if(slot == null || slot.IsAvailable) {
            Debug.LogWarning("The food box is not assigned to this table.");
            return;
        }

        Debug.Log($"Removing {foodBox.FoodName} from {name}...");

        if(_activeCookingCoroutines.TryGetValue(foodBox, out var cookingCoroutine)) {
            StopCoroutine(cookingCoroutine);
            _activeCookingCoroutines.Remove(foodBox);
        }

        foodBox.SetCookingTime(slot.RemainingTime);
        foodBox.Detach();
        foodBox.SetState(ItemState.Carried);
        slot.Clear();

        Debug.Log("Slot is now available for new food.");
    }

    protected abstract IEnumerator ProcessFood(CookingSlot slot);

    protected void CompleteCooking(CookingSlot slot) {
        if(slot.FoodBox == null) return;

        Debug.Log($"{slot.FoodBox.FoodName} has finished cooking on {name}!");
        slot.FoodBox.AdvanceCookingStage();
        slot.FoodBox.Detach();
        slot.FoodBox.SetState(ItemState.Cooked);
        slot.Clear();
    }

    private CookingSlot GetAvailableSlot(FoodBoxObject foodBox = null) {
        // Clear any existing slot assignments for the food box
        if(foodBox != null) {
            foreach(var slot in _cookingSlots) {
                if(slot.FoodBox == foodBox) {
                    slot.Clear();
                }
            }
        }

        return _cookingSlots.Find(s => s.IsAvailable);
    }

    public void DisplayRemainingTimes() {
        foreach(var slot in _cookingSlots) {
            if(!slot.IsAvailable) {
                Debug.Log($"Food: {slot.FoodBox.FoodName}, Remaining Time: {slot.RemainingTime:F1}s");
            }
        }
    }
    #endregion

    #region CookingSlot
    public class CookingSlot {
        public Transform Spot { get; }
        public FoodBoxObject FoodBox { get; private set; }
        public float RemainingTime { get; private set; }

        public bool IsAvailable => FoodBox == null;

        public CookingSlot(Transform spot) {
            Spot = spot;
            Clear();
        }

        public void AssignFoodBox(FoodBoxObject foodBox, float duration) {
            FoodBox = foodBox;
            RemainingTime = duration;
        }

        public void Clear() {
            FoodBox = null;
            RemainingTime = 0f;
        }

        public void UpdateTime(float deltaTime) {
            if(RemainingTime > 0) {
                RemainingTime -= deltaTime;
            }
        }
    }
    #endregion
}