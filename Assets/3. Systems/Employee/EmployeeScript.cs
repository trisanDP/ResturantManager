using UnityEngine;
using NPCSystem;

namespace NPCSystem {
    public class EmployeeScript : MonoBehaviour {
        #region Fields
        public EmployeeData empData;
        public EmployeeDatabase database;
        #endregion

        #region Unity Lifecycle Methods
        private void Start() {
            database = Resources.Load<EmployeeDatabase>("EmployeeDatabase");
            if(empData == null) {
                Debug.Log("NPCdata is null");
            }
            name = empData.npcName;
            empData.isUsed++;
            if(empData.isUsed > 1) {
                Debug.Log($"{empData.npcName} is being used {empData.isUsed} times");
                Destroy(gameObject);
            }
            database.RegisterActiveNPCs(this);
        }

        private void OnValidate() {
            if(empData != null) {
                name = empData.npcName;
                empData.conectedNPC = this;
            }
        }

        private void OnDestroy() {
            if(empData != null) {
                empData.isUsed = 0;
            }
        }
        #endregion
    }
}
