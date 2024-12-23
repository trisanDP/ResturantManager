using UnityEngine;
using System.Collections.Generic;

public class Table : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("Table Settings")]
    public Transform[] FoodPlacementPoints; // Points where cooked food can be placed
    public Transform[] NPCChairs;           // Chairs for NPCs

    private FoodBoxObject[] _placedFoods;
    private NPC[] _seatedNPCs;

    #endregion

    #region Initialization

    private void Start() {
        _placedFoods = new FoodBoxObject[FoodPlacementPoints.Length];
        _seatedNPCs = new NPC[NPCChairs.Length];
    }

    #endregion

    #region IInteractable Methods

    public void OnFocusEnter() {
        Debug.Log("Focusing on table");
    }

    public void OnFocusExit() {
        Debug.Log("Stopped focusing on table");
    }

    public void Interact(BoxController controller) {
        if(controller.HasCarriedBox()) {
            PlaceFood(controller);
        } else if(controller.HasSelectedNPC()) {
            SeatSpecificNPC(controller.GetSelectedNPC());
            controller.DeselectNPC();
        } else {
            Debug.LogWarning("Nothing to interact with: No food or NPC selected.");
        }
    }

    #endregion

    #region Food Management

    // Places food on an available placement point
    private void PlaceFood(BoxController controller) {
        FoodBoxObject carriedBox = controller.GetCarriedBox();

        if(carriedBox.CurrentCookingState != CookingState.Cooked) {
            Debug.LogWarning("Only fully cooked food can be placed on the table.");
            return;
        }

        for(int i = 0; i < FoodPlacementPoints.Length; i++) {
            if(_placedFoods[i] == null) {
                _placedFoods[i] = carriedBox;
                carriedBox.Attach(FoodPlacementPoints[i], Vector3.zero, Quaternion.identity);
                carriedBox.SetCookingState(CookingState.Cooked);
                controller.ClearCarriedBox();
                Debug.Log("Food placed on the table.");
                NotifySeatedNPCs();
                return;
            }
        }

        Debug.LogWarning("Table is full of food!");
    }

    // Provides an available food box for an NPC
    public FoodBoxObject GetAvailableFood() {
        for(int i = 0; i < _placedFoods.Length; i++) {
            if(_placedFoods[i] != null) {
                FoodBoxObject food = _placedFoods[i];
                _placedFoods[i] = null; // Remove food from table
                return food;
            }
        }

        return null; // No food available
    }

    #endregion

    #region NPC Seating

    // Seats a specific NPC at an available chair
    public void SeatSpecificNPC(NPC selectedNPC) {
        if(selectedNPC == null) {
            Debug.LogWarning("No NPC selected to seat.");
            return;
        }

        for(int i = 0; i < NPCChairs.Length; i++) {
            if(_seatedNPCs[i] == null) {
                _seatedNPCs[i] = selectedNPC;
                selectedNPC.TeleportTo(NPCChairs[i].position, NPCChairs[i].rotation);
                selectedNPC.AssignTable(this);
                Debug.Log($"NPC {selectedNPC.name} seated at chair {i}.");
                return;
            }
        }

        Debug.LogWarning("All chairs at the table are occupied.");
    }

    // Notifies seated NPCs to check for food
    private void NotifySeatedNPCs() {
        foreach(var npc in _seatedNPCs) {
            if(npc != null) {
                npc.CheckForFoodOnTable();
            }
        }
    }

    #endregion
}
