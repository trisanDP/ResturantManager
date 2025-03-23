using UnityEngine;
using TMPro;
using System.Collections.Generic;

#region QuestUIManager Class
public class QuestUIManager : MonoBehaviour {
    #region Variables


    public GameObject taskItemPrefab;      // Prefab for a single quest UI item (should have a TextMeshProUGUI component)
    public Transform activeQuestListParent; // Parent transform for active/incomplete quest items
    public Transform completedQuestListParent; // Parent transform for completed quest items
    private List<Quest> activeQuests = new List<Quest>();   // Local cache for active quests
    private List<Quest> completedQuests = new List<Quest>(); // Local cache for completed quests
    #endregion

    #region Unity Methods
    private void Awake() {

    }
    #endregion

    private void Start() {
        QuestManager.Instance.OnQuestCompleted += OnQuestCompleted;
        UpdateTasksUI();
    }

    private void OnQuestCompleted(Quest quest) {
        UpdateTasksUI();
    }

    #region UI Update Methods
    public void UpdateTasksUI() {
        // Clear current UI elements from both lists
        Debug.Log("Calling");
        ClearList(activeQuestListParent);
        ClearList(completedQuestListParent);

        // Get active quests from QuestManager
        activeQuests = QuestManager.Instance.activeQuests;
        completedQuests = QuestManager.Instance.GetCompletedQuests(); // Ensure QuestManager exposes this

        // Instantiate UI items for active quests
        foreach(var quest in activeQuests) {
            GameObject taskItem = Instantiate(taskItemPrefab, activeQuestListParent);
            TextMeshProUGUI textComp = taskItem.GetComponentInChildren<TextMeshProUGUI>();
            if(textComp != null)
                textComp.text = $"{quest.questName}\n{quest.description}";
        }

        // Instantiate UI items for completed quests
        foreach(var quest in completedQuests) {
            GameObject taskItem = Instantiate(taskItemPrefab, completedQuestListParent);
            TextMeshProUGUI textComp = taskItem.GetComponentInChildren<TextMeshProUGUI>();
            if(textComp != null)
                textComp.text = $" {quest.questName}\nCompleted";
        }
    }

    private void ClearList(Transform parent) {
        foreach(Transform child in parent)
            Destroy(child.gameObject);
    }
    #endregion
}
#endregion
