using UnityEngine;

public class BoxController : MonoBehaviour {

    public Transform CarryPoint;
    private FoodBoxObject _carriedBox;

    public bool HasCarriedBox() {
        return _carriedBox != null;
    }

    public FoodBoxObject GetCarriedBox() {
        return _carriedBox;
    }

    public void ClearCarriedBox() {
        _carriedBox = null;
    }

    public void PickUpBox(FoodBoxObject box) {
        if(_carriedBox != null) {
            Debug.LogWarning("Already carrying a box!");
            return;
        }

        _carriedBox = box;
        box.Attach(CarryPoint, Vector3.zero, Quaternion.identity);
        box.SetState(ItemState.Carried);
    }

    public void DropBox() {
        if(_carriedBox == null) {
            Debug.LogWarning("No box to drop.");
            return;
        }

        _carriedBox.Detach();
        _carriedBox.SetState(ItemState.Idle);
        _carriedBox = null;
    }
}
