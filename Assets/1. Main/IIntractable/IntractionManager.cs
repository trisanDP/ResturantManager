using UnityEngine;

[RequireComponent(typeof(BoxController))]
public class InteractionManager : MonoBehaviour {
    [Header("Crosshair Interaction Settings")]
    public Camera PlayerCamera; // Assign the player's camera
    public float RaycastDistance = 5f;
    public LayerMask InteractableLayer;

    private IInteractable _currentInteractable;
    private BoxController _boxController;

    private void Awake() {
        _boxController = GetComponent<BoxController>();
    }

    private void Update() {
        DetectInteractableWithRaycast();

        // Interact with the object
        if(Input.GetKeyDown(KeyCode.E)) {
            _currentInteractable?.Interact(_boxController);
        }

        // Drop the carried box
        if(Input.GetKeyDown(KeyCode.C)) {
            _boxController.DropBox();
        }
    }

    private void DetectInteractableWithRaycast() {
        // Raycast from the center of the screen
        Ray ray = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(Physics.Raycast(ray, out RaycastHit hit, RaycastDistance, InteractableLayer)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            // Handle interaction focus
            if(interactable != _currentInteractable) {
                _currentInteractable?.OnFocusExit();
                _currentInteractable = interactable;
                _currentInteractable?.OnFocusEnter();
            }
        } else {
            // Clear focus if no interactable is found
            _currentInteractable?.OnFocusExit();
            _currentInteractable = null;
        }
    }
}