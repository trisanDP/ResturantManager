using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BoxObject : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("Box Settings")]
    public InteractionState CurrentInteractionState = InteractionState.Idle;

    private Transform _originalParent;

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

    #endregion

    #region Attachment Methods

    // Attaches the box to a parent transform
    public void Attach(Transform parent, Vector3 offset, Quaternion rotation) {
        _originalParent = transform.parent;
        transform.SetParent(parent, true);
        transform.localPosition = offset;
        transform.localRotation = rotation;

        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = true;
        }
    }

    // Detaches the box from its parent
    public void Detach() {
        transform.SetParent(null, true);
        if(TryGetComponent(out Rigidbody rb)) {
            rb.isKinematic = false;
        }
    }

    #endregion
}