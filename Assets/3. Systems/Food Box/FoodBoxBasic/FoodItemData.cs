using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Food Item")]
public class FoodItemData : ScriptableObject {
    [Header("Food Details")]
    public string FoodName;
    public CookingStage[] CookingStages;
    public FoodQuality CurrentQuality;
    public int cost;
/*    public GameObject prefab; // Associated prefab*/

    public CookingStage GetCurrentCookingStage(int stageIndex) {
        if(stageIndex >= 0 && stageIndex < CookingStages.Length) {
            return CookingStages[stageIndex];
        }
        return null;
    }

    [System.Serializable]
    public class CookingStage {
        public string StageName;
        public float Duration;
        public CookingTableType RequiredTableType;
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