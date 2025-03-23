using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "Restaurant/Food Item Data")]
public class FoodItemData : ScriptableObject {
    [Header("Food Details")]
    public string FoodName;
    public string FoodID;         // Unique identifier for this food item
    public int RequiredLevel;

    public int costPrice;
    public int sellPrice;

    public int unlockCost;        // Cost to unlock the item (if applicable)

    public CookingStage[] CookingStages;
    public FoodQuality CurrentQuality;

    public CookingStage GetCurrentCookingStage(int stageIndex) {
        if(stageIndex >= 0 && stageIndex < CookingStages.Length) {
            return CookingStages[stageIndex];
        }
        return null;
    }

    public void OnValidate() {
        if(CookingStages != null && CookingStages.Length != 0) {
            foreach(var stage in CookingStages) {
                stage.StageName = stage.RequiredTableType.ToString();
            }
        }
    }

    [System.Serializable]
    public class CookingStage {
        public string StageName;
        public CookingTableType RequiredTableType;
        public float Duration;
    }

    public enum FoodQuality {
        Low, Mid, High
    }
}



// Represents different cooking-related states
public enum CookingState {
    Raw,
    Cooking,
    HalfCooked,
    Cooked
}

// Represents different interaction-related states
public enum InteractionState {
    Idle,
    Carried,
    Stored,
    CookingPlaced
}