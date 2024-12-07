using UnityEngine;

public class Shelf : MonoBehaviour, IInteractable {
    [Header("Shelf Settings")]
    [Tooltip("The position where the box will be placed on the shelf.")]
    public Transform StoragePoint;

    private BoxObject _storedBox;

    public void OnFocusEnter() {
        // Provide feedback when the player looks at the shelf
        Debug.Log("Looking at shelf");
    }

    public void OnFocusExit() {
        // Remove feedback when the player looks away
        Debug.Log("Stopped looking at shelf");
    }

    public void Interact() {
        // Check if the player is carrying a box
        PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        if(playerInteraction == null) return;

        BoxObject carriedBox = playerInteraction.GetCarriedBox();
        if(carriedBox != null) {
            if(_storedBox == null) {
                StoreBox(carriedBox);
                playerInteraction.ClearCarriedBox();
            } else {
                Debug.LogWarning("Shelf is already holding a box!");
            }
        } else {
            Debug.LogWarning("Player is not carrying a box!");
        }
    }

    private void StoreBox(BoxObject box) {
        Debug.Log("Storing box on the shelf");
        _storedBox = box;

        // Attach box to the shelf and position it
        box.transform.SetParent(StoragePoint, true);
        box.transform.localPosition = Vector3.zero;
        box.transform.localRotation = Quaternion.identity;

        // Disable physics for the stored box
        Rigidbody rb = box.GetComponent<Rigidbody>();
        if(rb != null) rb.isKinematic = true;
    }
}
