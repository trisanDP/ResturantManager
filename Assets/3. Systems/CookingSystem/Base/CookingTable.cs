using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CookingTable : MonoBehaviour, IInteractable {
    #region Fields and Properties
    [Header("Cooking Table Settings")]
    public Transform[] CookingSpots;
    public CookingTableType TableType;

    private readonly List<CookingSlot> _cookingSlots = new();
    private readonly Dictionary<FoodObject, Coroutine> _activeCookingCoroutines = new();
    #endregion

    #region Unity Methods
    private void Awake() {
        foreach(var spot in CookingSpots) {
            var slot = new CookingSlot(spot);
            _cookingSlots.Add(slot);
/*            CookingManager.Instance.RegisterSlot(slot);*/
        }
    }

/*    private void OnDestroy() {
        foreach(var slot in _cookingSlots) {
            CookingManager.Instance.UnregisterSlot(slot);
        }
    }*/
    #endregion

    #region Interaction Methods
    public virtual void OnFocusEnter() {
        Debug.Log($"Focusing on {name}");
    }

    public virtual void OnFocusExit() {
        Debug.Log($"Stopped focusing on {name}");
    }

    public void Interact(BoxController controller) {
        var carriedBox = controller.GetCarriedBox();
        FoodObject carriedFoodBox = carriedBox.GetComponent<FoodObject>();
        if(carriedBox != null && carriedBox.CurrentInteractionState == InteractionState.Carried) {
            TryStartCooking(carriedFoodBox, controller);
        } else {
            Debug.Log("No food box is being carried or invalid state.");
        }
    }
    #endregion

    #region Cooking Management
    protected virtual void TryStartCooking(FoodObject foodBox, BoxController controller) {
        if(foodBox == null || foodBox.FoodItem == null) {
            Debug.LogWarning("Invalid food box or food item!");
            return;
        }

        var currentStage = foodBox.FoodItem.GetCurrentCookingStage(foodBox.CurrentStageIndex);
        if(currentStage == null || currentStage.RequiredTableType != TableType) {
            Debug.LogWarning($"This table cannot handle {foodBox.FoodName} at the current stage.");
            return;
        }

        var availableSlot = GetAvailableSlot();
        if(availableSlot == null) {
            Debug.LogWarning("No available cooking spots.");
            return;
        }

        float cookingTime = foodBox.RemainingCookingTime > 0 ? foodBox.RemainingCookingTime : currentStage.Duration;
        PlaceFoodBoxOnCookingTable(foodBox, controller, availableSlot, cookingTime);
    }

    private void PlaceFoodBoxOnCookingTable(FoodObject foodObject, BoxController controller, CookingSlot slot, float duration) {
        BoxObject BoxObject = foodObject.GetComponent<BoxObject>();
        BoxObject.Attach(slot.Spot, Vector3.zero, Quaternion.identity);
        foodObject.SetCookingState(CookingState.Cooking);
        BoxObject.SetInteractionState(InteractionState.CookingPlaced);
        slot.AssignFoodBox(foodObject, duration);
        controller.ClearCarriedBox();

        Coroutine cookingCoroutine = StartCoroutine(ProcessFood(slot));
        _activeCookingCoroutines[foodObject] = cookingCoroutine;
    }

    public void RemoveFoodBox(BoxObject BoxObject) {
        var slot = _cookingSlots.Find(s => s.FoodBox == BoxObject);
        if(slot == null || slot.IsAvailable) {
            Debug.LogWarning("The food box is not assigned to this table.");
            return;
        }
        FoodObject foodBox = BoxObject.GetComponent<FoodObject>();

        if(_activeCookingCoroutines.TryGetValue(foodBox, out var coroutine)) {
            StopCoroutine(coroutine);
            _activeCookingCoroutines.Remove(foodBox);
        }

        foodBox.SetCookingTime(slot.RemainingTime);
        BoxObject.Detach();
        BoxObject.SetInteractionState(InteractionState.Carried);
        slot.Clear();
    }

    protected abstract IEnumerator ProcessFood(CookingSlot slot);

    private CookingSlot GetAvailableSlot() {
        return _cookingSlots.Find(s => s.IsAvailable);
    }
    #endregion
}



#region CookingSlot

public class CookingSlot {
    public Transform Spot { get; }
    public FoodObject FoodBox { get; private set; }
    public float RemainingTime { get; private set; }
    public float TotalCookingTime { get; private set; }  // Added property for total cooking time

    public bool IsAvailable => FoodBox == null;

    public CookingSlot(Transform spot) {
        Spot = spot;
        Clear();
    }

    public void AssignFoodBox(FoodObject foodBox, float duration) {
        FoodBox = foodBox;
        RemainingTime = duration;
        TotalCookingTime = duration;  // Set the total cooking time
    }

    public void Clear() {
        FoodBox = null;
        RemainingTime = 0f;
        TotalCookingTime = 0f;  // Reset the total cooking time when clearing the slot
    }

    public void UpdateTime(float deltaTime) {
        if(RemainingTime > 0) {
            RemainingTime -= deltaTime;
        }
    }
}

#endregion
public enum CookingTableType {
    PrepTable,
    Stove,
    Oven
}