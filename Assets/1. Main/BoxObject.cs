using UnityEngine;

public class BoxObject : MonoBehaviour, IInteractable {
    [Header("Carry Settings")]
    [Tooltip("Position offset when the box is carried.")]
    public Vector3 CarryOffset = new Vector3(0, -0.5f, 1f);

    [Tooltip("Rotation offset when the box is carried.")]
    public Vector3 CarryRotation = Vector3.zero;

    private Transform _originalParent;
    private bool _isBeingCarried = false;
    public Transform _carrier;

    public void OnFocusEnter() {
        // Highlight or provide feedback when the player looks at the box
        Debug.Log("Looking at box");
    }

    public void OnFocusExit() {
        // Remove highlight or feedback when the player looks away
        Debug.Log("Stopped looking at box");
    }

    public void Interact() {
        if(_isBeingCarried) {
            Debug.LogWarning("Box is already being carried!");
            return;
        }
        if(_IsStroed)

        Debug.Log("Picking up the box");
        CarryBox();
    }

    private void CarryBox() {
        _originalParent = transform.parent;
        _isBeingCarried = true;

        // Attach box to the player (carrier)
        transform.SetParent(_carrier, true);
        transform.localPosition = CarryOffset;
        transform.localRotation = Quaternion.Euler(CarryRotation);

        // Disable physics while carrying
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = true;
    }

    public void DropBox() {
        if(!_isBeingCarried) return;

        Debug.Log("Dropping the box");
        _isBeingCarried = false;

        // Detach from the player
        transform.SetParent(_originalParent, true);

        // Re-enable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = false;
    }

    public void SetCarrier(Transform carrier) {
        _carrier = carrier;
    }

    public bool IsBeingCarried() {
        return _isBeingCarried;
    }
}
