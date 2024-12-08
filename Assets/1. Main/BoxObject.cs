using UnityEngine;

public class BoxObject : MonoBehaviour, IInteractable {
    #region Fields and Settings
    public Vector3 CarryOffset = new Vector3(0, -0.5f, 1f); // Position offset when carried
    public Vector3 CarryRotation = Vector3.zero; // Rotation offset when carried

    private Transform _originalParent;
    private bool _isBeingCarried = false;
    private bool _isStored = false;
    #endregion

    #region IInteractable Implementation
    public void OnFocusEnter() {
        Debug.Log("Looking at the box");
    }

    public void OnFocusExit() {
        Debug.Log("Stopped looking at the box");
    }

    public void Interact() {
        if(_isBeingCarried) return;

        PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();
        if(player != null) {
            if(_isStored) {
                // If the box is stored, remove it from the shelf first
                Shelf shelf = GetComponentInParent<Shelf>();
                if(shelf != null) {
                    shelf.RemoveBox(this);
                }
            }

            player.PickUpBox(this);
        }
    }
    #endregion

    #region Box Behavior
    public void PickUp(Transform carrier) {
        _isBeingCarried = true;
        _isStored = false;
        _originalParent = transform.parent;

        // Attach box to the player
        transform.SetParent(carrier, true);
        transform.localPosition = CarryOffset;
        transform.localRotation = Quaternion.Euler(CarryRotation);

        // Disable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = true;

        Debug.Log("Box picked up");
    }

    public void Drop() {
        if(!_isBeingCarried) return;

        _isBeingCarried = false;
        transform.SetParent(null, true);

        // Enable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = false;

        Debug.Log("Box dropped");
    }

    public void Store(Transform storagePoint) {
        _isStored = true;
        _isBeingCarried = false;

        // Attach box to the shelf
        transform.SetParent(storagePoint, true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Disable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = true;

        Debug.Log("Box stored");
    }

    public void ResetToDefault() {
        _isStored = false;
        transform.SetParent(null, true);

        // Enable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = false;

        Debug.Log("Box reset to default");
    }
    #endregion
}
