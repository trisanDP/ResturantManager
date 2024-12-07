using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    [Header("Interaction Settings")]
    [Tooltip("Maximum distance for interaction.")]
    public float InteractionDistance = 3f;

    [Tooltip("Layer mask for interactable objects.")]
    public LayerMask InteractableLayer;

    [Tooltip("Key to trigger interaction.")]
    public KeyCode InteractionKey = KeyCode.E;

    [Header("UI Settings")]
    [Tooltip("UI Image representing the interaction dot.")]
    public Image InteractionDot;

    [Tooltip("Color of the dot when no interactable is in focus.")]
    public Color DefaultDotColor = Color.white;

    [Tooltip("Color of the dot when looking at an interactable.")]
    public Color InteractableDotColor = Color.blue;

    [Header("Dependencies")]
    [Tooltip("Camera used to raycast from the center of the screen.")]
    public Camera PlayerCamera;

    private IInteractable _currentInteractable;

    [Tooltip("The point where carried objects are attached.")]
    public Transform CarryPoint;

    private BoxObject _carriedBox;

    private void Start() {
        // Ensure PlayerCamera is assigned
        if(PlayerCamera == null) {
            PlayerCamera = Camera.main;
        }

        // Ensure InteractionDot is assigned
        if(InteractionDot == null) {
            Debug.LogError("InteractionDot is not assigned! Please assign it in the inspector.");
        }
    }


    private void Update() {
        HandleInteraction();
        HandleDrop();
    }

    #region Carry
    public BoxObject GetCarriedBox() {
        return _carriedBox;
    }

    public void ClearCarriedBox() {
        _carriedBox = null;
    }

    private void HandleDrop() {
        if(_carriedBox != null && Input.GetKeyDown(KeyCode.C)) {
            _carriedBox.DropBox();
            _carriedBox = null;
        }
    }
    #endregion

    private void HandleInteraction() {
        // Perform a raycast from the center of the screen
        Ray ray = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;


        if(Physics.Raycast(ray, out hit, InteractionDistance, InteractableLayer)) {
            // Check if the object hit by the ray is interactable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            Debug.Log("Intract");
            if(interactable != null) {
                // Update the interaction dot color to indicate focus
                InteractionDot.color = InteractableDotColor;
                Debug.Log("here");

                // If we are looking at a new interactable object
                if(interactable != _currentInteractable) {
                    _currentInteractable?.OnFocusExit(); // Notify previous interactable
                    _currentInteractable = interactable;
                    _currentInteractable.OnFocusEnter();
                }
                if(_currentInteractable is BoxObject boxObject && !_carriedBox) {
                    boxObject.SetCarrier(CarryPoint);
                    Debug.Log("Calling");
                    _carriedBox = boxObject;
                }
                // Handle interaction input
                if(Input.GetKeyDown(InteractionKey)) {
                    _currentInteractable.Interact();
                }
                return; // Exit here to avoid resetting the dot color
            }
        }

        // Reset the dot color and notify the current interactable if no object is hit
        if(_currentInteractable != null) {
            _currentInteractable.OnFocusExit();
            _currentInteractable = null;
        }
        InteractionDot.color = DefaultDotColor;


    }
}