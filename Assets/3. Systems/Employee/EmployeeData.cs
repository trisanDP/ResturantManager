using Mono.Cecil;
using NPCSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/New NPC")]
public class EmployeeData : ScriptableObject {
    public string npcName;                      // NPC name
    public Sprite portrait;                     // NPC portrait
    [TextArea(3, 10)] public string[] dialogues; // NPC dialogue lines
    public int friendshipLevel;                 // Friendship level of the NPC
    public int isUsed;

    public EmployeeScript conectedNPC;

    // Reference to the NPC database
    [SerializeField] private EmployeeDatabase empDatabase;

    // Flag to check if the NPC is already registered
    private bool isRegistered = false;

    public float deliveryTimeMultiplier = 1f; // To calculate delivery time
    public int currentMoral = 10;             // Moral ranges from 1 to 10

    // Automatically registers the NPC when necessary
    private void OnValidate() {
        if(!isRegistered) {
            RegisterIfNeeded();
        }

        Debug.Log("From Here");
        // Initialize resources in storage from ResourceDatabase
    }


    #region Register
    // Registers the NPC if it isn't registered and database is not null
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

    // Context menu option to manually register the NPC to the database
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


