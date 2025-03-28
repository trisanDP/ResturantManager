using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestaurantManagement {
    public class ShopProgressManager : MonoBehaviour {
        public static ShopProgressManager Instance { get; private set; }
        private HashSet<string> unlockedItemIDs = new();
        private const string UnlockedItemsKey = "UnlockedFoodItems";

        private void Awake() {
            if(Instance == null) {
                Instance = this;
                LoadProgress();
            } else {
                Destroy(gameObject);
            }
        }

        // Loads unlocked items from PlayerPrefs or uses default items from the database.
        public void LoadProgress(ItemDatabase itemDatabase = null) {
            unlockedItemIDs.Clear();
            if(PlayerPrefs.HasKey(UnlockedItemsKey)) {
                string savedData = PlayerPrefs.GetString(UnlockedItemsKey);
                string[] ids = savedData.Split(',');
                foreach(var id in ids) {
                    if(!string.IsNullOrEmpty(id))
                        unlockedItemIDs.Add(id);
                }
            } else if(itemDatabase != null) {
                // If no saved progress, unlock default items if provided.
                foreach(var item in itemDatabase.defaultMenuItems) {
                    if(!string.IsNullOrEmpty(item.FoodID))
                        unlockedItemIDs.Add(item.FoodID);
                }
                SaveProgress();
            }
        }

        // Checks if a given item is unlocked.
        public bool IsItemUnlocked(string itemID) {
            return unlockedItemIDs.Contains(itemID);
        }

        // Unlocks an item and saves progress.
        public void UnlockItem(string itemID) {
            if(unlockedItemIDs.Add(itemID)) {
                Debug.Log($"Item {itemID} unlocked!");
                SaveProgress();
            }
        }

        // Saves the unlocked items to PlayerPrefs.
        public void SaveProgress() {
            string data = string.Join(",", unlockedItemIDs);
            PlayerPrefs.SetString(UnlockedItemsKey, data);
            PlayerPrefs.Save();
        }

        // Provides access to the list of unlocked IDs.
        public IEnumerable<string> GetUnlockedIDs() => unlockedItemIDs;
    }
}
