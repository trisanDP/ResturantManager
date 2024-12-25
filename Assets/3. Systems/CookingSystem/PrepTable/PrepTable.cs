using UnityEngine;
using System.Collections;

public class PrepTable : CookingTable {

    protected override IEnumerator ProcessFood(CookingSlot slot) {
        if(slot.FoodBox == null) {
            Debug.LogError("No FoodBox assigned to the slot!");
            yield break;
        }

        Debug.Log($"Preparing the {slot.FoodBox.FoodName} in the PrepTable...");

        while(slot.RemainingTime > 0) {
            yield return null;
            slot.UpdateTime(Time.deltaTime);
        }

        Debug.Log($"{slot.FoodBox.FoodName} is done in the PrepTable!");
/*        CompleteCooking(slot);*/
    }
}
