using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FoodBoxObject : MonoBehaviour, IInteractable {
    [Header("Box Settings")]
    public ItemState CurrentState = ItemState.Idle;

    [Header("Food Settings")]
    public FoodItem FoodItem;
    public string FoodName;
    public float RemainingCookingTime { get; private set; } = 0f;
    public int CurrentStageIndex { get; private set; } = 0;

    private Transform _originalParent;

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
        if(CurrentState == ItemState.Carried) {
            Debug.LogWarning($"{name} is already being carried!");
            return;
        }

        Debug.Log($"Interacting with {name}");
        controller.PickUpBox(this);
    }
    #endregion

    #region State Management
    public void SetState(ItemState state) {
        CurrentState = state;
    }

    public void SetCookingTime(float remainingTime) {
        RemainingCookingTime = remainingTime;
    }

    public bool IsFullyCooked() {
        return CurrentStageIndex >= FoodItem.CookingStages.Length;
    }

    public void AdvanceCookingStage() {
        RemainingCookingTime = 0f; // Reset cooking time
        CurrentStageIndex++;
        if(IsFullyCooked()) {
            Debug.Log($"{FoodName} is fully cooked and ready to serve!");
            SetState(ItemState.Ready);
        } else {
            Debug.Log($"{FoodName} is ready for the next stage.");
        }
    }
    #endregion

    #region Attachment Methods
    public void Attach(Transform parent, Vector3 offset, Quaternion rotation) {
        _originalParent = transform.parent;
        transform.SetParent(parent, true);
        transform.localPosition = offset;
        transform.localRotation = rotation;

        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = true;
        }
    }

    public void Detach() {
        transform.SetParent(null, true);

        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = false;
        }
    }
    #endregion
}