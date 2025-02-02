using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RestaurantManagement;

public class Table : MonoBehaviour, IInteractable {
    #region Fields and Properties

    [Header("Table Settings")]
    public int ChairCapacity = 4;
    public Transform[] FoodPlacementPoints;
    public Transform[] chairs;

    private FoodObject[] _placedFoods;
    private Customer[] _seatedNPCs;
    private TableOrder _tableOrder;

    #endregion

    #region Initialization

    private void Start() {
        _placedFoods = new FoodObject[FoodPlacementPoints.Length];
        _seatedNPCs = new Customer[chairs.Length];
        _tableOrder = new TableOrder();
    }

    #endregion

    #region IInteractables
    public void OnFocusEnter() {
        // Debug.Log($"Focusing on table: {name}");
    }

    public void OnFocusExit() {
        // Debug.Log($"Stopped focusing on table: {name}");
    }

    public void Interact(BoxController controller) {
        if(controller.HasCarriedBox()) {
            PlaceFood(controller);
        } else if(controller.HasSelectedNPC()) {
            Customer selectedNPC = controller.GetSelectedNPC();
            SeatNPC(selectedNPC, GetRandomAvailableChairIndex());
            controller.DeselectNPC();
        }
    }
    #endregion

    #region Order Management

    public void TakeOrder(Customer npc, FoodItemData foodItem) {
        _tableOrder.AddOrder(npc, foodItem);
    }
    public void PlaceFood(BoxController controller) {
        var carriedBox = controller.GetCarriedBox();
        var foodBox = carriedBox.GetComponent<FoodObject>();

        if(foodBox.CurrentCookingState != CookingState.Cooked) {
            Debug.LogWarning("Only fully cooked food can be placed on the table.");
            return;
        }

        Customer npc = _tableOrder.GetNPCForFood(foodBox.FoodItemData);
        if(npc == null) {
            Debug.LogWarning("No NPC at this table ordered this food.");
            return;
        }

        int npcIndex = System.Array.IndexOf(_seatedNPCs, npc);
        if(npcIndex == -1) {
            Debug.LogWarning($"{npc.name} is not seated at this table.");
            return;
        }

        _placedFoods[npcIndex] = foodBox;
        carriedBox.Attach(FoodPlacementPoints[npcIndex], Vector3.zero, Quaternion.identity);
        controller.ClearCarriedBox();

        Debug.Log($"{foodBox.FoodName} placed in front of {npc.name}.");

        // Notify the NPC to start eating
        npc.NotifyFoodPlaced(foodBox);
    }

    #endregion

    #region Chair Management

    public void SeatNPC(Customer npc, int chairIndex) {
        if(chairIndex < 0 || chairIndex >= chairs.Length || _seatedNPCs[chairIndex] != null) {
            Debug.LogWarning("Invalid or occupied chair.");
            return;
        }

        _seatedNPCs[chairIndex] = npc;
        npc.TeleportTo(chairs[chairIndex].position, chairs[chairIndex].rotation);
        npc.AssignTable(this);
    }

    public void ClearNPC(int chairIndex) {
        if(chairIndex < 0 || chairIndex >= chairs.Length) return;

        _seatedNPCs[chairIndex] = null;
    }

    public void SeatNPCsRandomly(Customer[] npcs) {
        if(npcs == null || npcs.Length == 0) {
            Debug.LogWarning("No NPCs provided to seat.");
            return;
        }

        // Create a list of available chair indices and shuffle it
        List<int> availableChairs = GetAvailableChairIndices();
        availableChairs = availableChairs.OrderBy(_ => Random.value).ToList();

        int npcIndex = 0;

        foreach(int chairIndex in availableChairs) {
            if(npcIndex >= npcs.Length) break; // Stop if all NPCs are seated

            SeatNPC(npcs[npcIndex], chairIndex);
            npcIndex++;
        }

        if(npcIndex < npcs.Length) {
            Debug.LogWarning("Not all NPCs could be seated. Some chairs are occupied or insufficient.");
        }
    }

    private List<int> GetAvailableChairIndices() {
        List<int> availableChairs = new List<int>();

        for(int i = 0; i < chairs.Length; i++) {
            if(_seatedNPCs[i] == null) {
                availableChairs.Add(i);
            }
        }

        return availableChairs;
    }

    private int GetRandomAvailableChairIndex() {
        List<int> availableChairs = GetAvailableChairIndices();

        if(availableChairs.Count == 0) {
            Debug.LogWarning("No available chairs.");
            return -1;
        }

        return availableChairs[Random.Range(0, availableChairs.Count)];
    }

    #endregion
}
