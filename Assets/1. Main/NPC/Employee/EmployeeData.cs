using UnityEngine;

namespace NPCSystem {
    [CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/New NPC")]
    public class EmployeeData : ScriptableObject {
        public string npcName;                      // NPC name
        public Sprite portrait;                     // NPC portrait
        [TextArea(3, 10)] public string[] dialogues; // NPC dialogue lines
        public int friendshipLevel;                 // Friendship level of the NPC
        public int isUsed;

        public EmployeeScript conectedNPC;

        [SerializeField] private EmployeeDatabase empDatabase;

        private bool isRegistered = false;

        public float deliveryTimeMultiplier = 1f; // To calculate delivery time
        public int currentMoral = 10;             // Moral ranges from 1 to 10

        private void OnValidate() {
            if(!isRegistered) {
                RegisterIfNeeded();
            }
            Debug.Log("From Here");
        }

        #region Registration Methods
        private void RegisterIfNeeded() {
            if(empDatabase == null) {
                empDatabase = Resources.Load<EmployeeDatabase>("EmployeeDatabase");
                if(empDatabase == null) {
                    Debug.LogError("EmployeeDatabase not found in Resources. Please place an EmployeeDatabase in the Resources folder.");
                    return;
                }
            }
            RegisterToDatabase();
        }

        [ContextMenu("Register NPC to Database")]
        public void RegisterToDatabase() {
            if(empDatabase == null) {
                Debug.LogWarning("NPC Database is not assigned.");
                return;
            }
            if(empDatabase.IsNPCDataRegistered(this)) {
                Debug.LogWarning($"{npcName} is already registered in the database.");
                isRegistered = true;
                return;
            }
            empDatabase.RegisterNPCData(this);
            Debug.Log($"{npcName} successfully registered to the database.");
            isRegistered = true;
        }
        #endregion
    }
}
