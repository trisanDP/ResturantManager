
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item Database")]
public class ItemDatabase : ScriptableObject {
    public List<FoodItemData> allItems; // All items available in the game
    public List<FoodItemData> activeItems; // Items currently available in the shop
}
