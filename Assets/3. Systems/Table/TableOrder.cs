using UnityEngine;
using System.Collections.Generic;

public class TableOrder {
    public Dictionary<NPC, FoodItem> Orders { get; private set; } = new Dictionary<NPC, FoodItem>();

    public void AddOrder(NPC npc, FoodItem foodItem) {
        if(Orders.ContainsKey(npc)) {
            Debug.LogWarning($"{npc.name} already has an order.");
            return;
        }
        Orders[npc] = foodItem;
        Debug.Log($"{npc.name} ordered {foodItem.FoodName}.");
    }

    public void RemoveOrder(NPC npc) {
        if(Orders.Remove(npc)) {
            Debug.Log($"{npc.name}'s order has been removed.");
        }
    }

    public FoodItem GetOrder(NPC npc) {
        return Orders.TryGetValue(npc, out var foodItem) ? foodItem : null;
    }

    public NPC GetNPCForFood(FoodItem foodItem) {
        foreach(var order in Orders) {
            if(order.Value == foodItem) {
                return order.Key;
            }
        }
        return null;
    }

    public bool HasOrder(NPC npc) {
        return Orders.ContainsKey(npc);
    }
}
