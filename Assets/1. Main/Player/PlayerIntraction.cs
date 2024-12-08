using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    #region Fields and Settings
    [Header("Interaction Settings")]
    public float InteractionRange = 3f; // Distance for interacting with objects
    public LayerMask InteractableLayer; // Layer mask for interactable objects

    private BoxObject _carriedBox;
    #endregion

    #region Unity Methods
    void Update() {
        HandleInput();
    }
    #endregion

    #region Input Handling
    private void HandleInput() {
        if(Input.GetKeyDown(KeyCode.E)) {
            Interact();
        } else if(Input.GetKeyDown(KeyCode.C)) {
            DropBox();
        }
    }
    #endregion

    #region Interaction Logic
    private void Interact() {
        // Raycast to find interactable objects
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, InteractionRange, InteractableLayer)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null) {
                if(_carriedBox == null) {
                    // Interact with the object (e.g., pick up a box)
                    interactable.Interact();
                } else {
                    // Attempt to store the carried box if interacting with a shelf
                    Shelf shelf = hit.collider.GetComponent<Shelf>();
                    if(shelf != null) {
                        shelf.StoreBox(_carriedBox);
                        ClearCarriedBox();
                    }
                }
            }
        }
    }

    private void DropBox() {
        if(_carriedBox == null) return;

        // Drop the box to the ground
        _carriedBox.Drop();
        ClearCarriedBox();
    }
    #endregion

    #region Box Management
    public BoxObject GetCarriedBox() {
        return _carriedBox;
    }

    public void ClearCarriedBox() {
        _carriedBox = null;
    }

    public void PickUpBox(BoxObject box) {
        _carriedBox = box;
        box.PickUp(transform);
    }
    #endregion
}
