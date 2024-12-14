/*using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    #region Fields and Settings
    [Tooltip("The radius within which the player can interact with objects.")]
    public float InteractionRange = 2f;

    [Tooltip("Layer mask to identify interactable objects.")]
    public LayerMask InteractableLayer;

    private IInteractable _currentInteractable;
    private BoxObject _carriedBox;
    public Transform CarryPoint; // Where the box will attach when carried
    #endregion

    #region MonoBehaviour Methods
    private void Update() {
        CheckForInteractables();

        if(Input.GetKeyDown(KeyCode.E)) {
            if(_currentInteractable != null) {
                _currentInteractable.Interact();
            }
        }

        if(Input.GetKeyDown(KeyCode.C)) {
            DropBox();
        }
    }
    #endregion

    #region Interaction Methods
    private void CheckForInteractables() {
        // Perform a sphere overlap to detect interactable objects
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, InteractionRange, InteractableLayer);

        if(hitColliders.Length > 0) {
            IInteractable interactable = hitColliders[0].GetComponent<IInteractable>();
            if(interactable != _currentInteractable) {
                if(_currentInteractable != null) {
                    _currentInteractable.OnFocusExit();
                }

                _currentInteractable = interactable;
                if(_currentInteractable != null) {
                    _currentInteractable.OnFocusEnter();
                }
            }
        } else {
            if(_currentInteractable != null) {
                _currentInteractable.OnFocusExit();
                _currentInteractable = null;
            }
        }
    }

    public void PickUpBox(BoxObject box) {
        if(_carriedBox != null) {
            Debug.LogWarning("Already carrying a box!");
            return;
        }

        _carriedBox = box;
        AttachBox(box);
        box.SetState(ItemState.Carried);
        Debug.Log($"Picked up {box.ItemData.ItemName}");
    }

    public void DropBox() {
        if(_carriedBox == null) {
            Debug.Log("No box to drop.");
            return;
        }

        DetachBox(_carriedBox);
        _carriedBox.SetState(ItemState.Idle);
        _carriedBox = null;
        Debug.Log("Dropped the box.");
    }

    public void StoreBox(Shelf shelf) {
        if(_carriedBox == null) {
            Debug.Log("No box to store.");
            return;
        }

        if(!shelf.StoreBox(_carriedBox)) {
            Debug.LogWarning("Shelf is full!");
            return;
        }

        DetachBox(_carriedBox);
        _carriedBox.SetState(ItemState.Stored);
        _carriedBox = null;
        Debug.Log("Box stored on shelf.");
    }

    public BoxObject GetCarriedBox() {
        return _carriedBox;
    }
    #endregion

    #region Attachment Methods
    private void AttachBox(BoxObject box) {
        box.transform.SetParent(CarryPoint, true);
        box.transform.localPosition = Vector3.zero;
        box.transform.localRotation = Quaternion.identity;

        Rigidbody rb = box.GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = true;
    }

    private void DetachBox(BoxObject box) {
        box.transform.SetParent(null, true);

        Rigidbody rb = box.GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = false;
    }
    #endregion
}
*/