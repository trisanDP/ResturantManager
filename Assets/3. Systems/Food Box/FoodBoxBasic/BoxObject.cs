using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoxObject : MonoBehaviour, IInteractable {
    [Header("Box Settings")]
    public InteractionState CurrentInteractionState = InteractionState.Idle;

    private Transform _originalParent;
    public FoodObject FoodObject;

    private void Start() {
        FoodObject = GetComponent<FoodObject>();
    }

    public void OnFocusEnter() {
        //Debug.Log($"Focusing on {name}");
    }

    public void OnFocusExit() {
        //Debug.Log($"Stopped focusing on {name}");
    }

    public void Interact(BoxController controller) {
        if(CurrentInteractionState == InteractionState.Carried) {
            Debug.LogWarning($"{name} is already being carried!");
            return;
        }

        if(FoodObject.CurrentCookingState == CookingState.Cooking) {
            Debug.LogWarning("It's still cooking");
            return;
        }

        //Debug.Log($"Interacting with {name}");
        controller.PickUpBox(this);
    }

    public void SetInteractionState(InteractionState state) {
        CurrentInteractionState = state;
    }

    // Attaches the box to a parent transform (e.g., to be carried)
    public void Attach(Transform parent, Vector3 offset, Quaternion rotation) {
        _originalParent = transform.parent;
        transform.SetParent(parent, true);
        transform.localPosition = offset;
        transform.localRotation = rotation;

        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = true;
        }
    }

    // Detaches the box so it can be dropped or interacted with again.
    public void Detach() {
        transform.SetParent(null, true);
        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = false;
        }
    }
}
