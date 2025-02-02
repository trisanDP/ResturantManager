using UnityEngine;
using System.Collections.Generic;
using RestaurantManagement;

public class TableOrder {
    public Dictionary<Customer, FoodItemData> Orders { get; private set; } = new Dictionary<Customer, FoodItemData>();

    public void AddOrder(Customer npc, FoodItemData foodItem) {
        if(Orders.ContainsKey(npc)) {
            Debug.LogWarning($"{npc.name} already has an order.");
            return;
        }
        Orders[npc] = foodItem;
        Debug.Log($"{npc.name} ordered {foodItem.FoodName}.");
    }

    public void RemoveOrder(Customer npc) {
        if(Orders.Remove(npc)) {
            Debug.Log($"{npc.name}'s order has been removed.");
        }
    }

    public FoodItemData GetOrder(Customer npc) {
        return Orders.TryGetValue(npc, out var foodItem) ? foodItem : null;
    }

    public Customer GetNPCForFood(FoodItemData foodItem) {
        foreach(var order in Orders) {
            if(order.Value == foodItem) {
                return order.Key;
            }
        }
        return null;
    }

    public bool HasOrder(Customer npc) {
        return Orders.ContainsKey(npc);
    }
}
