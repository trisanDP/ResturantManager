using UnityEngine;
using System.Collections.Generic;

public class Shelf : MonoBehaviour, IInteractable {
    #region Fields and Settings
    public List<Transform> StoragePoints = new List<Transform>(); // Positions to place boxes on the shelf

    private List<BoxObject> _storedBoxes = new List<BoxObject>();
    #endregion

    #region IInteractable Implementation
    public void OnFocusEnter() {
        Debug.Log("Looking at the shelf");
    }

    public void OnFocusExit() {
        Debug.Log("Stopped looking at the shelf");
    }

    public void Interact() {
        PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();
        if(player != null) {
            BoxObject carriedBox = player.GetCarriedBox();

            if(carriedBox != null) {
                StoreBox(carriedBox);
                player.ClearCarriedBox();
            } else {
                Debug.LogWarning("Player is not carrying any box");
            }
        }
    }
    #endregion

    #region Shelf Behavior
    public void StoreBox(BoxObject box) {
        if(_storedBoxes.Count >= StoragePoints.Count) {
            Debug.LogWarning("Shelf is full!");
            return;
        }

        _storedBoxes.Add(box);

        // Assign the next available storage point
        Transform storagePoint = StoragePoints[_storedBoxes.Count - 1];
        box.Store(storagePoint);

        Debug.Log("Box stored on the shelf");
    }

    public void RemoveBox(BoxObject box) {
        if(_storedBoxes.Contains(box)) {
            _storedBoxes.Remove(box);
            box.ResetToDefault();
            Debug.Log("Box removed from the shelf");
        }
    }
    #endregion
}
