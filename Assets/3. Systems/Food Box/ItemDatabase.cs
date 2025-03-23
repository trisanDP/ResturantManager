using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item Database")]
public class ItemDatabase : ScriptableObject {
    public List<FoodItemData> allItems;       // All items available in the game
    public List<FoodItemData> defaultMenuItems; // Items that should be unlocked by default

    public FoodItemData GetItemByName(string name) {
        return allItems.Find(item => item.FoodName == name);
    }
}
