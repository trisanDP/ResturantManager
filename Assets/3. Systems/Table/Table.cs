using UnityEngine;
using System.Collections.Generic;
using log4net;

public class Table : MonoBehaviour, IInteractable {
    [Header("Table Settings")]
    public Transform[] FoodPlacementPoints; // Points where cooked food can be placed
    public Transform[] NPCChairs; // Chairs for NPCs

    private FoodBoxObject[] _placedFoods;
    private NPC[] _seatedNPCs;

    private void Start() {
        _placedFoods = new FoodBoxObject[FoodPlacementPoints.Length];
        _seatedNPCs = new NPC[NPCChairs.Length];
    }

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
        } else{
            SeatNPC(controller);
        }
    }
    #endregion

    #region Food Management
    private void PlaceFood(BoxController controller) {
        FoodBoxObject carriedBox = controller.GetCarriedBox();
        if(!carriedBox.IsFullyCooked()) {
            Debug.LogWarning("Only fully cooked food can be placed on the table.");
            return;
        }

        for(int i = 0; i < FoodPlacementPoints.Length; i++) {
            if(_placedFoods[i] == null) {
                _placedFoods[i] = carriedBox;
                carriedBox.Attach(FoodPlacementPoints[i], Vector3.zero, Quaternion.identity);
                carriedBox.SetState(ItemState.Ready);
                controller.ClearCarriedBox();
                Debug.Log("Food placed on the table.");
                return;
            }
        }

        Debug.LogWarning("Table is full of food!");
    }
    #endregion

    #region NPC Seating
    private void SeatNPC(BoxController controller) {
        for(int i = 0; i < NPCChairs.Length; i++) {
            if(_seatedNPCs[i] == null) {
                NPC newNPC = SpawnNPC();
                _seatedNPCs[i] = newNPC;
                newNPC.TeleportTo(NPCChairs[i].position, NPCChairs[i].rotation);
                AssignFoodToNPC(newNPC);
                Debug.Log("NPC seated at the table.");
                return;
            }
        }

        Debug.LogWarning("Table is full of NPCs!");
    }

    private void AssignFoodToNPC(NPC npc) {
        for(int i = 0; i < _placedFoods.Length; i++) {
            if(_placedFoods[i] != null) {
                npc.StartEating(_placedFoods[i]);
                _placedFoods[i] = null;
                return;
            }
        }

        Debug.Log("No food available for the NPC.");
    }

    public NPC SpawnNPC() {
        GameObject npcPrefab = Resources.Load<GameObject>("NPCPrefab"); // Ensure the prefab exists in Resources folder
        return Instantiate(npcPrefab).GetComponent<NPC>();
    }
    #endregion
}