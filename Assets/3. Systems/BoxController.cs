using UnityEngine;

public class BoxController : MonoBehaviour {
    #region Fields and Properties

    [Header("Box Carry Settings")]
    public Transform CarryPoint; // Point where the carried box is held
    private BoxObject _carriedBox;

    [Header("NPC Selection Settings")]
    private Customer _selectedNPC;

    #endregion

    #region Box Management

    public bool HasCarriedBox() {
        return _carriedBox != null;
    }


    public BoxObject GetCarriedBox() {
        return _carriedBox;
    }

    public void ClearCarriedBox() {
        _carriedBox = null;
    }

    public void PickUpBox(BoxObject box) {
        if(_carriedBox != null) {
            Debug.LogWarning("Already carrying a box!");
            return;
        }

        _carriedBox = box;
        box.Attach(CarryPoint, Vector3.zero, Quaternion.identity);
        box.SetInteractionState(InteractionState.Carried);
    }

    public void DropBox() {
        if(_carriedBox == null) {
            Debug.LogWarning("No box to drop.");
            return;
        }

        _carriedBox.Detach();
        _carriedBox.SetInteractionState(InteractionState.Idle);
        _carriedBox = null;
    }

    #endregion

    #region NPC Selection

    public bool HasSelectedNPC() {
        return _selectedNPC != null;
    }

    public Customer GetSelectedNPC() {
        return _selectedNPC;
    }

    public void SelectNPC(Customer npc) {
        if(_selectedNPC != null) {
            Debug.LogWarning("An NPC is already selected.");
            return;
        }

        _selectedNPC = npc;
        Debug.Log($"NPC {npc.name} has been selected.");
    }

    public void DeselectNPC() {
        if(_selectedNPC == null) {
            Debug.LogWarning("No NPC is currently selected.");
            return;
        }

        Debug.Log($"NPC {_selectedNPC.name} has been deselected.");
        _selectedNPC = null;
    }

    #endregion
}