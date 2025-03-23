using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestDatabase", menuName = "Quest System/Quest Database")]
public class QuestDatabase : ScriptableObject {

    // This list will show up in the Inspector for easy viewing/editing.
    public List<Quest> allQuests = new List<Quest>();

    public List<Quest> dayQuest = new List<Quest>();
    public List<Quest> nightQuest = new List<Quest>();

    // Dictionary for fast quest lookups by ID.
    private Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();

    private void OnValidate() {
        // Ensure the dictionary is in sync with the list when changes happen in the Inspector.
        SyncDictionaryWithList();
    }

    // Syncs the dictionary with the list
    private void SyncDictionaryWithList() {
        questDictionary.Clear();
        foreach(Quest quest in allQuests) {
            if(!questDictionary.ContainsKey(quest.questID)) {
                questDictionary.Add(quest.questID, quest);
            } else {
                Debug.LogWarning($"Duplicate Quest ID found: {quest.questID}. Only the first instance will be used.");
            }
        }
    }

    // Add a new quest to both the list and the dictionary
    public void AddQuest(Quest newQuest) {
        if(!questDictionary.ContainsKey(newQuest.questID)) {
            allQuests.Add(newQuest);
            questDictionary.Add(newQuest.questID, newQuest);
        } else {
            Debug.LogError("Quest ID already exists.");
        }
    }

    // Get a quest by ID using the dictionary for fast lookups
    public Quest GetQuestByID(string questID) {
        questDictionary.TryGetValue(questID, out Quest quest);
        return quest;
    }

    // Get a quest by name (this uses the list since names may not be unique)
    public Quest GetQuestByName(string questName) {
        return allQuests.Find(q => q.questName == questName);
    }

    // Remove a quest from both the list and dictionary
    public void RemoveQuestByID(string questID) {
        Quest questToRemove = GetQuestByID(questID);
        if(questToRemove != null) {
            allQuests.Remove(questToRemove);
            questDictionary.Remove(questID);
        }
    }
}
