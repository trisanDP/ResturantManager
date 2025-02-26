using System.Collections.Generic;
using UnityEngine;

namespace RestaurantManagement {
    public class Player : MonoBehaviour,IUpdateObserver {
        #region Properties
        public decimal PersonalBalance = 1000m;
        public decimal BusinessBalance = 5000m;
        public SkillTree PlayerSkillTree = new SkillTree();
        #endregion

        #region Methods
        public void PurchaseFoodItem(FoodItemData item, int quantity) {
            // Example purchasing calculation based on quality and quantity.
            int cost = item.cost * quantity;
            if(PersonalBalance >= cost) {
                PersonalBalance -= cost;
                Debug.Log($"Purchased {quantity} of {item.FoodName} for {cost} units.");
            } else {
                Debug.Log("Not enough funds to purchase food item.");
            }
        }

        public void HireEmployee(NPCSystem.EmployeeScript employee) {
            Debug.Log($"Hired employee: {employee.empData.npcName}");
        }
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


    public class SkillTree {
        #region Fields
        public Dictionary<string, int> Skills = new Dictionary<string, int>();
        #endregion

        #region Constructor
        public SkillTree() {
            // Initialize default skills
            Skills["Motivation"] = 1;
            Skills["CustomerPrediction"] = 1;
        }
        #endregion

        #region Methods
        public void UpgradeSkill(string skillName) {
            if(Skills.ContainsKey(skillName)) {
                Skills[skillName]++;
                Debug.Log($"{skillName} upgraded to level {Skills[skillName]}.");
            } else {
                Debug.LogWarning($"Skill {skillName} does not exist.");
            }
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

