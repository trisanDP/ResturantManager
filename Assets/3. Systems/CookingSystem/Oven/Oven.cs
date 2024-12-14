using System.Collections;
using UnityEngine;



public class Oven : CookingTable {
    protected override IEnumerator ProcessFood(CookingSlot slot) {
        if(slot.FoodBox == null) {
            Debug.LogError("No FoodBox assigned to the slot!");
            yield break;
        }

        Debug.Log($"Cooking {slot.FoodBox.FoodName} in the Oven...");

        while(slot.RemainingTime > 0) {
            yield return null;
            slot.UpdateTime(Time.deltaTime);

            // Stop cooking if the food box is removed from the table
            if(slot.IsAvailable || slot.FoodBox.CurrentState != ItemState.Cooking) {
                Debug.Log($"Cooking interrupted for {slot.FoodBox.FoodName}.");
                yield break;
            }
        }

        Debug.Log($"{slot.FoodBox.FoodName} is done in the Oven!");
        CompleteCooking(slot);
    }
}
