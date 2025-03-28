using UnityEngine;

#region EnvironmentManager Class
public class EnvironmentManager : MonoBehaviour {
    public static EnvironmentManager Instance { get; private set; }

    [Header("Manager References")]
    public ShelfManager shelfManager;
    public CookingStationManager cookingStationManager;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            // Optionally auto-assign if not set in Inspector:
            if(shelfManager == null) {
                shelfManager = ShelfManager.Instance;
            }
            if(cookingStationManager == null) {
                cookingStationManager = CookingStationManager.Instance;
            }
        } else {
            Destroy(gameObject);
        }
    }

    // Unified query for a shelf.
    public Shelf GetShelfFor(FoodItemData foodData) {
        return shelfManager != null ? shelfManager.GetShelfFor(foodData) : null;
    }

    // Unified query for a cooking station.
    public CookingStation GetStationFor(FoodItemData foodData) {
        return cookingStationManager != null ? cookingStationManager.GetStationFor(foodData) : null;
    }
}
#endregion
