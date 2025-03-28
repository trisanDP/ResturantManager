using System.Collections.Generic;
using UnityEngine;

#region CookingStationManager Class
public class CookingStationManager : MonoBehaviour {
    public static CookingStationManager Instance { get; private set; }
    private List<CookingStation> stations = new List<CookingStation>();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void Register(CookingStation station) {
        if(!stations.Contains(station)) {
            stations.Add(station);
        }
    }

    // Returns a cooking station suitable for the food item based on its current cooking stage.
    public CookingStation GetStationFor(FoodItemData foodData) {
        if(foodData == null || foodData.CookingStages == null || foodData.CookingStages.Length == 0)
            return null;

        // For simplicity, we check the first stage's required table type.
        CookingStationType requiredType = foodData.CookingStages[0].RequiredTableType;
        foreach(var station in stations) {
            if(station.TableType == requiredType) {
                return station;
            }
        }
        return null;
    }
}
#endregion
