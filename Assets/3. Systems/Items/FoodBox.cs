/*using UnityEngine;
using System.Collections;
using static FoodItem;

public class FoodBox : MonoBehaviour {
    public FoodItem foodItem; // Assigned in the inspector (or dynamically)
    private int currentStageIndex = 0;

    public string FoodName => foodItem.FoodName;

    public void StartCooking() {
        if(foodItem.CookingStages.Length > 0) {
            StartCoroutine(ProcessNextStage());
        }
    }

    private IEnumerator ProcessNextStage() {
        if(currentStageIndex < foodItem.CookingStages.Length) {
            CookingStage currentStage = foodItem.CookingStages[currentStageIndex];
            CookingTable table = currentStage.AssociatedTable;

            // Execute preparation (if any) and cooking stages
            Debug.Log($"Starting {currentStage.StageName} for {FoodName}...");

            yield return new WaitForSeconds(currentStage.Duration); // Time to complete stage

            Debug.Log($"{FoodName} completed stage: {currentStage.StageName}.");

            // Proceed to the next stage
            currentStageIndex++;

            // Continue processing the next stage if applicable
            if(currentStageIndex < foodItem.CookingStages.Length) {
                StartCoroutine(ProcessNextStage());
            }
        } else {
            Debug.Log($"{FoodName} is fully cooked!");
            // Finished cooking, the food is ready for serving or other actions
        }
    }
}
*/