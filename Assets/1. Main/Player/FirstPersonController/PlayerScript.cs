using System.Collections.Generic;
using UnityEngine;

namespace RestaurantManagement {
    public class Player : MonoBehaviour,IUpdateObserver {
        #region Properties
        #endregion

        #region Methods
        #endregion

        #region Inputs 
        private bool isTabOpen = false;

        public void ObservedUpdate() {
            ObserveInput();
        }

        void ObserveInput() {
            if(Input.GetKeyDown(KeyCode.Tab)) {
                isTabOpen = !isTabOpen;
                Debug.Log("Pressed");
                EventManager.Trigger("TabToggle", isTabOpen);
            }
        }



        void OnEnable() {
            UpdateManager.RegisterObserver(this);
        }
        void OnDestroy() {
            UpdateManager.UnregisterObserver(this);
        }
        #endregion
    }

    public class RuleBook {
        #region Fields
        // Example: Rules and their descriptions/effects.
        public Dictionary<string, string> Rules = new Dictionary<string, string>();
        #endregion

        #region Constructor
        public RuleBook() {
            // Initialize with default rules.
            Rules["TipSharing"] = "Waiters share tips equally.";
            Rules["DoubleCleaning"] = "Increases cleanliness but decreases employee loyalty.";
        }
        #endregion

        #region Methods
        public void SetRule(string ruleName, string ruleDescription) {
            if(Rules.ContainsKey(ruleName))
                Rules[ruleName] = ruleDescription;
            else
                Rules.Add(ruleName, ruleDescription);
        }
        #endregion
    }
}

