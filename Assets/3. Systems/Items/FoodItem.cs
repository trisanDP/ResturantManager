using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Food Item")]
public class FoodItem : ScriptableObject {
    [Header("Food Details")]
    public string FoodName;
    public CookingStage[] CookingStages;
    public FoodQuality CurrentQuality;

    public CookingStage GetCurrentCookingStage(int stageIndex) {
        if(stageIndex >= 0 && stageIndex < CookingStages.Length) {
            return CookingStages[stageIndex];
        }
        return null;
    }

    [System.Serializable]
    public class CookingStage {
        [Header("Stage Details")]
        public string StageName; // e.g., "Preparation", "Stove", "Oven"
        public float Duration;   // Duration in seconds for this stage
        public CookingTableType RequiredTableType; // Reference to the type of table needed
    }

    public enum FoodQuality {
        Low,Mid,High
    }
}


public enum ItemState {
    Idle,
    Carried,
    Stored,
    Cooking,
    Cooked,
    CookednCarried,
    Ready
}
