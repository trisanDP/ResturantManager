using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem {
    [CreateAssetMenu(fileName = "EmployeeDatabase", menuName = "NPC/NPC Database")]
    public class EmployeeDatabase : ScriptableObject {
        public List<EmployeeData> npcDataList = new();
        public List<EmployeeScript> ActiveNPCLists;
        public int creditAmount;

        public bool IsNPCDataRegistered(EmployeeData npc) {
            return npcDataList.Contains(npc);
        }

        public bool IsActiveNPCRegistered(EmployeeScript npc) {
            return ActiveNPCLists.Contains(npc);
        }

        public void RegisterNPCData(EmployeeData npc) {
            if(!IsNPCDataRegistered(npc)) {
                npcDataList.Add(npc);
            }
        }

        public void RegisterActiveNPCs(EmployeeScript npc) {
            if(!IsActiveNPCRegistered(npc)) {
                ActiveNPCLists.Add(npc);
            }
            RegisterNPCData(npc.empData);
        }
    }
}
