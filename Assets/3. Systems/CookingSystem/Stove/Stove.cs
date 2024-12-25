using System.Collections;
using UnityEngine;

public class Stove : CookingTable {
    protected override IEnumerator ProcessFood(CookingSlot slot) {
        if(slot.FoodBox == null)
            yield break;

        var foodBox = slot.FoodBox;
        var currentStage = foodBox.FoodItem?.GetCurrentCookingStage(foodBox.CurrentStageIndex);

        if(currentStage == null)
            yield break;

        Debug.Log($"Cooking {foodBox.FoodName} on {name}...");

        float totalDuration = currentStage.Duration;
        float elapsedTime = 0f;

        while(elapsedTime < totalDuration && foodBox.CookingProgress < foodBox.MaxProgress) {
            yield return null;

            float deltaTime = Time.deltaTime;
            elapsedTime += deltaTime;
            slot.UpdateTime(deltaTime);

            float progressIncrement = (deltaTime / totalDuration) * 100f;
            foodBox.UpdateCookingProgress(progressIncrement);
        }

/*        CompleteCooking(slot);*/
    }


}
