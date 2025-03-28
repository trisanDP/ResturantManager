using System.Collections.Generic;
using UnityEngine;

#region ShelfManager Class
public class ShelfManager : MonoBehaviour {
    public static ShelfManager Instance { get; private set; }
    private List<Shelf> shelves = new List<Shelf>();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void RegisterShelf(Shelf shelf) {
        if(!shelves.Contains(shelf)) {
            shelves.Add(shelf);
        }
    }

    // Returns a shelf that can supply the given food.
    public Shelf GetShelfFor(FoodItemData foodData) {
        // Simplest implementation: return the first shelf found.
        return shelves.Count > 0 ? shelves[0] : null;
    }
}
#endregion
