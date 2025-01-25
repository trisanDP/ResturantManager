using log4net;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace NPCSystem {
    [CreateAssetMenu(fileName = "EmployeeDatabase", menuName = "NPC/NPC Database")]
    public class EmployeeDatabase : ScriptableObject {
        // A list that stores all registered NPCs
        public List<EmployeeData> npcDataList = new();
        public List<EmployeeScript> ActiveNPCLists;
        public int creditAmount;

        // Method to check if the NPC is already registered

        public bool IsNPCDataRegistered(EmployeeData npc) {
            return npcDataList.Contains(npc);
        }

        public bool IsActiveNPCRegistered(EmployeeScript npc) {
            return ActiveNPCLists.Contains(npc);
        }

        // Method to register a new NPC
        public void RegisterNPCData(EmployeeData npc) {
            if(!IsNPCDataRegistered(npc)) {
                npcDataList.Add(npc);
                /*            Debug.Log($"{npc.npcName} has been added to the NPC Database.");*/
            } else {
                /*            Debug.LogWarning($"{npc.npcName} is already registered.");*/
            }
        }

        public void RegisterActiveNPCs(EmployeeScript npc) {
            if(!IsActiveNPCRegistered(npc)) {
                ActiveNPCLists.Add(npc);
                /*            Debug.Log($"{npc.name} has been added to the NPC Database.");*/
            } else {
                /*            Debug.LogWarning($"{npc.name} is already registered.");*/
            }
            RegisterNPCData(npc.empData);
        }
    }
}