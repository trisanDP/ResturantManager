using UnityEngine;
using System.Collections.Generic;
using System;

namespace RestaurantManagement {
    public class TableManager {
        private List<Table> tables = new List<Table>();

        public event Action<Table> OnTableAvailabilityChanged;
        public bool HasAvailableSeat => HasAvailableTable();

        public void RegisterTable(Table table) {
            if(!tables.Contains(table)) {
                tables.Add(table);
                //Debug.Log($"Table Added: {table}");
            }
        }

        public void UnregisterTable(Table table) {
            tables.Remove(table);
        }

        // Returns a random table that has at least one available seat.
        public Table GetAvailableTable() {
            List<Table> availableTables = new List<Table>();
            foreach(Table table in tables) {
                if(table.HasAvailableSeat) {
                    availableTables.Add(table);
                }
            }
            if(availableTables.Count == 0) return null;
            int randomIndex = UnityEngine.Random.Range(0, availableTables.Count);
            return availableTables[randomIndex];
        }

        // Checks if there is any available table without reserving it.
        public bool HasAvailableTable() {
            foreach(Table table in tables) {
                if(table.HasAvailableSeat) {
                    return true;
                }
            }
            return false;
        }
        // Checks table availability via TableManager.
        public bool IsTableAvailable() {
            return RestaurantManager.Instance?.TableManager?.HasAvailableTable() ?? false;
        }

        public void NotifyTableAvailabilityChanged(Table table) {
            if(table == null) {
                Debug.LogWarning("NotifyTableAvailabilityChanged called with a null table.");
                return;
            }
            OnTableAvailabilityChanged?.Invoke(table);
        }


    }
}

