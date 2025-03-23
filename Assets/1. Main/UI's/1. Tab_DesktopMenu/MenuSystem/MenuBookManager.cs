using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestaurantManagement {
    public class MenuBookManager {
        #region Fields & Properties
        // A runtime copy of all food items from the database.
        public List<FoodItemData> AllAvailableItems { get; private set; } = new List<FoodItemData>();
        // Items unlocked during gameplay.
        public List<FoodItemData> UnlockedItems { get; private set; } = new List<FoodItemData>();
        // Items currently selected for the menu.
        public List<FoodItemData> MenuItems { get; private set; } = new List<FoodItemData>();
        // Maximum number of items allowed in the menu.
        public int MenuSlotLimit { get; private set; } = 8;
        #endregion

        #region Events
        // Event fired when the menu is updated.
        public event Action OnMenuUpdated = delegate { };
        #endregion

        #region Initialization
        // Initializes the MenuBookManager from the provided database asset.
        public void Initialize(ItemDatabase database) {
            if(database != null) {
                AllAvailableItems = new List<FoodItemData>(database.allItems);

                // By default, consider all items unlocked.
                if(database.allItems != null && database.allItems.Count > 0)
                    UnlockedItems = new List<FoodItemData>(database.allItems);

                // If there are default menu items, add them.
                if(database.defaultMenuItems != null) {
                    foreach(var item in database.defaultMenuItems) {
                        if(!MenuItems.Contains(item)) {
                            MenuItems.Add(item);
                            if(!UnlockedItems.Contains(item))
                                UnlockedItems.Add(item); // Ensure default items are unlocked.
                        }
                    }
                }
                OnMenuUpdated?.Invoke();
            }
        }
        #endregion

        #region Unlocking Methods
        // Unlocks a single food item.
        public void UnlockItem(FoodItemData item) {
            if(item != null && !UnlockedItems.Contains(item)) {
                UnlockedItems.Add(item);
                Debug.Log("Unlocked: " + item.FoodName);
                OnMenuUpdated?.Invoke();
            }
        }

        // Unlocks items based on the player's level.
        public void UnlockItemsByLevel(int playerLevel) {
            foreach(var item in AllAvailableItems) {
                if(item.RequiredLevel <= playerLevel && !UnlockedItems.Contains(item))
                    UnlockItem(item);
            }
        }
        #endregion

        #region Menu Operations
        // Returns a list of unlocked food items.
        public List<FoodItemData> GetUnlockedItems() {
            List<FoodItemData> unlocked = new List<FoodItemData>();
            foreach(var item in AllAvailableItems) {
                if(UnlockedItems.Contains(item))
                    unlocked.Add(item);
            }
            return unlocked;
        }

        // Adds an item to the menu if conditions are met.
        public void AddItemToMenu(FoodItemData item) {
            if(item == null) return;
            // Only allow modifications if the restaurant is closed.
            if(RestaurantStateManager.Instance != null &&
                RestaurantStateManager.Instance.CurrentState != RestaurantState.Closed) {
                Debug.Log("Cannot modify menu when restaurant is open.");
                return;
            }
            if(!UnlockedItems.Contains(item)) {
                Debug.Log("Item not unlocked: " + item.FoodName);
                return;
            }
            if(MenuItems.Contains(item)) {
                Debug.Log("Item already in menu: " + item.FoodName);
                return;
            }
            if(MenuItems.Count >= MenuSlotLimit) {
                Debug.Log("Menu is full.");
                return;
            }
            MenuItems.Add(item);
            Debug.Log("Added to Menu: " + item.FoodName);
            OnMenuUpdated?.Invoke();
        }

        // Removes an item from the menu.
        public void RemoveItemFromMenu(FoodItemData item) {
            if(item == null) return;
            if(RestaurantStateManager.Instance != null &&
                RestaurantStateManager.Instance.CurrentState != RestaurantState.Closed) {
                Debug.Log("Cannot modify menu when restaurant is open.");
                return;
            }
            if(MenuItems.Contains(item)) {
                MenuItems.Remove(item);
                Debug.Log("Removed from Menu: " + item.FoodName);
                OnMenuUpdated?.Invoke();
            }
        }
        #endregion

        #region Persistence Methods
        // Saves menu and unlocked items using PlayerPrefs.
        public void SaveState() {
            List<string> menuIDs = new List<string>();
            foreach(var item in MenuItems)
                menuIDs.Add(item.FoodID);
            PlayerPrefs.SetString("MenuItems", JsonUtility.ToJson(new StringListWrapper(menuIDs)));

            List<string> unlockedIDs = new List<string>();
            foreach(var item in UnlockedItems)
                unlockedIDs.Add(item.FoodID);
            PlayerPrefs.SetString("UnlockedItems", JsonUtility.ToJson(new StringListWrapper(unlockedIDs)));

            PlayerPrefs.Save();
            Debug.Log("Menu state saved.");
        }

        // Loads menu and unlocked items from PlayerPrefs.
        public void LoadState() {
            bool loadedUnlocked = false;
            if(PlayerPrefs.HasKey("UnlockedItems")) {
                StringListWrapper unlockedWrapper = JsonUtility.FromJson<StringListWrapper>(PlayerPrefs.GetString("UnlockedItems"));
                UnlockedItems.Clear();
                foreach(var id in unlockedWrapper.List) {
                    FoodItemData item = GetItemByID(id);
                    if(item != null)
                        UnlockedItems.Add(item);
                }
                loadedUnlocked = true;
            }
            if(!loadedUnlocked) {
                Debug.Log("No saved unlocked items; using default unlocked items.");
            }

            bool loadedMenu = false;
            if(PlayerPrefs.HasKey("MenuItems")) {
                StringListWrapper menuWrapper = JsonUtility.FromJson<StringListWrapper>(PlayerPrefs.GetString("MenuItems"));
                MenuItems.Clear();
                foreach(var id in menuWrapper.List) {
                    FoodItemData item = GetItemByID(id);
                    if(item != null && UnlockedItems.Contains(item))
                        MenuItems.Add(item);
                }
                loadedMenu = true;
            }
            if(!loadedMenu) {
                Debug.Log("No saved menu items; using default menu items.");
            }
            Debug.Log("Menu state loaded.");
        }
        #endregion

        #region Utility Methods
        // Finds a food item by its FoodID from AllAvailableItems.
        private FoodItemData GetItemByID(string id) {
            FoodItemData item = AllAvailableItems.Find(x => x.FoodID == id);
            if(item == null)
                Debug.LogWarning("Item with ID " + id + " not found.");
            return item;
        }
        #endregion
    }

    // Helper class for serializing lists of strings.
    [Serializable]
    public class StringListWrapper {
        public List<string> List;
        public StringListWrapper(List<string> list) {
            List = list;
        }
    }
}
