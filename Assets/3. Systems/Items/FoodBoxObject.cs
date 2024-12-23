using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FoodBoxObject : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("Box Settings")]
    public InteractionState CurrentInteractionState = InteractionState.Idle;

    [Header("Food Settings")]
    public CookingState CurrentCookingState = CookingState.Raw;
    public FoodItem FoodItem;
    public string FoodName;
    public float RemainingCookingTime { get; private set; } = 0f;
    public int CurrentStageIndex { get; private set; } = 0;

    private Transform _originalParent;

    #endregion

    public float CookingProgress { get; private set; } = 0f;
    public float MaxProgress => FoodItem != null && FoodItem.CookingStages != null ? FoodItem.CookingStages.Length * 100f : 0f;


    public void UpdateCookingProgress(float increment) {
        CookingProgress += increment;
        if(CookingProgress >= MaxProgress) {
            CookingProgress = MaxProgress;
            SetCookingState(CookingState.Cooked);
            Debug.Log($"{FoodName} is fully cooked!");
        }
    }

    #region Unity Methods

    private void OnValidate() {
        if(FoodItem != null) {
            FoodName = FoodItem.FoodName;
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
        if(CurrentInteractionState == InteractionState.Carried) {
            Debug.LogWarning($"{name} is already being carried!");
            return;
        }

        Debug.Log($"Interacting with {name}");
        controller.PickUpBox(this);
    }

    #endregion

    #region State Management

    // Sets the interaction state
    public void SetInteractionState(InteractionState state) {
        CurrentInteractionState = state;
    }

    // Sets the cooking state
    public void SetCookingState(CookingState state) {
        CurrentCookingState = state;
    }

    // Sets the remaining cooking time
    public void SetCookingTime(float remainingTime) {
        RemainingCookingTime = remainingTime;
    }

    // Checks if the food is fully cooked
    public bool IsFullyCooked() {

        return CurrentStageIndex >= FoodItem.CookingStages.Length;
    }

    // Advances the cooking stage
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

    #region Attachment Methods

    // Attaches the food box to a parent transform
    public void Attach(Transform parent, Vector3 offset, Quaternion rotation) {
        _originalParent = transform.parent;
        transform.SetParent(parent, true);
        transform.localPosition = offset;
        transform.localRotation = rotation;

        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = true;
        }
    }

    // Detaches the food box from its parent
    public void Detach() {
        transform.SetParent(null, true);
        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = false;
        }
    }

    public void FinishedEating() {
        Destroy(gameObject);
    }

    #endregion
}