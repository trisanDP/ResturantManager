using System.Collections;
using UnityEngine;

public class Stove : CookingTable {
    protected override IEnumerator ProcessFood(CookingSlot slot) {
        if(slot.FoodBox == null) {
            Debug.LogError("No FoodBox assigned to the slot!");
            yield break;
        }

        Debug.Log($"Cooking {slot.FoodBox.FoodName} in the Oven...");

        while(slot.RemainingTime > 0) {
            yield return null;
            slot.UpdateTime(Time.deltaTime);
        }
        
        Debug.Log($"{slot.FoodBox.FoodName} is done in the Oven!");
        CompleteCooking(slot);
    }
}
