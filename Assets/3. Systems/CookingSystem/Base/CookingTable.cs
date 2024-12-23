using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class CookingTable : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("Cooking Table Settings")]
    public Transform[] CookingSpots;
    public CookingTableType TableType;

    private readonly List<CookingSlot> _cookingSlots = new();
    public readonly Dictionary<FoodBoxObject, Coroutine> _activeCookingCoroutines = new();

    #endregion

    #region Unity Methods

    private void Awake() {
        foreach(var spot in CookingSpots) {
            _cookingSlots.Add(new CookingSlot(spot));
        }
    }

    #endregion

    #region Interaction Methods

    public virtual void OnFocusEnter() {
        Debug.Log($"Focusing on {name}");
    }

    public virtual void OnFocusExit() {
        Debug.Log($"Stopped focusing on {name}");
    }

    public void Interact(BoxController controller) {
        FoodBoxObject carriedBox = controller.GetCarriedBox();

        if(carriedBox != null && carriedBox.CurrentInteractionState == InteractionState.Carried) {
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
        PlaceFoodBoxOnCookingTable(foodBox, controller, availableSlot, cookingTime);

    }

    private void PlaceFoodBoxOnCookingTable(FoodBoxObject foodBox, BoxController controller, CookingSlot slot, float duration) {
        foodBox.Attach(slot.Spot, Vector3.zero, Quaternion.identity);
        foodBox.SetCookingState(CookingState.Cooking);
        foodBox.SetInteractionState(InteractionState.CookingPlaced);
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

        if(_activeCookingCoroutines.TryGetValue(foodBox, out var coroutine)) {
            StopCoroutine(coroutine);
            _activeCookingCoroutines.Remove(foodBox);
        }

        foodBox.SetCookingTime(slot.RemainingTime);
        foodBox.Detach();
        foodBox.SetInteractionState(InteractionState.Carried);
        slot.Clear();


        Debug.Log("Slot is now available for new food.");
    }

    protected abstract IEnumerator ProcessFood(CookingSlot slot);

    protected void CompleteCooking(CookingSlot slot) {
        if(slot.FoodBox == null) return;

        Debug.Log($"{slot.FoodBox.FoodName} has finished cooking on {name}!");
        slot.FoodBox.AdvanceCookingStage();
        slot.FoodBox.Detach();
        slot.FoodBox.SetCookingState(CookingState.Cooked);
        slot.Clear();
    }

    private CookingSlot GetAvailableSlot(FoodBoxObject foodBox = null) {
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

public enum CookingTableType {
    PrepTable,
    Stove,
    Oven
}