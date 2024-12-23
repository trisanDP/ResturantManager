using UnityEngine;

public class Shelf : MonoBehaviour, IInteractable {
    public Transform[] StoragePoints;
    private FoodBoxObject[] _storedBoxes;

    private void Start() {
        _storedBoxes = new FoodBoxObject[StoragePoints.Length];
    }

    public void OnFocusEnter() {
        Debug.Log("Focusing on shelf");
    }

    public void OnFocusExit() {
        Debug.Log("Stopped focusing on shelf");
    }

    public void Interact(BoxController controller) {
        if(controller.HasCarriedBox()) {
            StoreBox(controller);
        } else {
            RetrieveBox(controller);
        }
    }

    private void StoreBox(BoxController controller) {
        FoodBoxObject carriedBox = controller.GetCarriedBox();
        for(int i = 0; i < StoragePoints.Length; i++) {
            if(!carriedBox.IsFullyCooked() && _storedBoxes[i] == null) {
                _storedBoxes[i] = carriedBox;
                carriedBox.Attach(StoragePoints[i], Vector3.zero, Quaternion.identity);
                carriedBox.SetInteractionState(InteractionState.Stored);
                controller.ClearCarriedBox();
                Debug.Log($"{carriedBox.name} stored on shelf.");
                return;
            }
        }

        Debug.LogWarning("Shelf is full!");
    }

    private void RetrieveBox(BoxController controller) {
        for(int i = 0; i < _storedBoxes.Length; i++) {
            if(_storedBoxes[i] != null) {
                FoodBoxObject box = _storedBoxes[i];
                _storedBoxes[i] = null;
                box.Detach();
                controller.PickUpBox(box);
                Debug.Log($"{box.name} retrieved from shelf.");
                return;
            }
        }

        Debug.Log("No boxes available on the shelf.");
    }
}
