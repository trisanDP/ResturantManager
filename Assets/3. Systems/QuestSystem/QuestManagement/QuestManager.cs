using RestaurantManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public static QuestManager Instance { get; private set; }
    public List<Quest> activeQuests = new List<Quest>();      // Active quests
    private List<Quest> completedQuests = new List<Quest>();    // Completed quests
    public event Action<Quest> OnQuestCompleted;              // Fired on quest completion

    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() {
        EventManager.OnMoneyEarned += HandleMoneyEarned;
        EventManager.OnMoneySpent += HandleMoneySpent;
    }

    private void OnDisable() {
        EventManager.OnMoneyEarned -= HandleMoneyEarned;
        EventManager.OnMoneySpent -= HandleMoneySpent;
    }

    // Activate a quest.
    public void ActivateQuest(Quest quest) {
        if(quest == null) {
            Debug.Log("Quest is null");
            return;
        }
        if(!activeQuests.Contains(quest)) {
            quest.StartQuest();
            activeQuests.Add(quest);
            //Debug.Log("Activated quest: " + quest.questID);
        }
    }

    // Update a specific quest's objective progress (if needed)
    public void UpdateQuestProgress(string questID, int objectiveIndex, int progress) {
        Quest quest = activeQuests.Find(q => q.questID == questID);
        if(quest != null && quest.status == Quest.QuestStatus.Active) {
            if(objectiveIndex >= 0 && objectiveIndex < quest.objectives.Count) {
                quest.objectives[objectiveIndex].UpdateProgress(progress);
                if(quest.CheckCompletion())
                    CompleteQuest(quest);
            }
        }
    }

    // New: Update all quests with an objective of a given type.
    public void UpdateQuestsByObjective(ObjectiveType objectiveType, int progress) {
        foreach(var quest in activeQuests.ToArray()) { // Use ToArray to safely iterate.
            for(int i = 0; i < quest.objectives.Count; i++) {
                if(quest.objectives[i].objectiveType == objectiveType) {
                    quest.objectives[i].UpdateProgress(progress);
                }
            }
            if(quest.CheckCompletion())
                CompleteQuest(quest);
        }
    }

    // For satisfaction objectives.
    public void UpdateSatisfactionProgress(string questID, int objectiveIndex, float satisfaction) {
        Quest quest = activeQuests.Find(q => q.questID == questID);
        if(quest != null && quest.status == Quest.QuestStatus.Active) {
            if(objectiveIndex >= 0 && objectiveIndex < quest.objectives.Count &&
                quest.objectives[objectiveIndex] is SatisfactionObjective so) {
                so.UpdateSatisfaction(satisfaction);
                if(quest.CheckCompletion())
                    CompleteQuest(quest);
            }
        }
    }

    // Mark a quest as complete.
    private void CompleteQuest(Quest quest) {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        Debug.Log("Completed quest: " + quest.questID);
        ProcessRewards(quest);
        OnQuestCompleted?.Invoke(quest);
    }

    // Processes rewards for a completed quest.
    private void ProcessRewards(Quest quest) {
        foreach(var reward in quest.rewards) {
            switch(reward.rewardType) {
                case QuestReward.RewardType.Cash:
                FinanceManager fm = RestaurantManager.Instance.FinanceManager;
                if(fm != null)
                    fm.AddBusinessIncome(reward.amount, "Quest Reward: " + quest.questName);
                break;
                case QuestReward.RewardType.Reputation:
                RestaurantManager rm = RestaurantManager.Instance;
                if(rm != null) {
                    rm.Popularity += (float)reward.amount;
                    Debug.Log("Reputation increased by " + reward.amount + " RP");
                }
                break;
            }
        }
    }

    public List<Quest> GetCompletedQuests() {
        return new List<Quest>(completedQuests);
    }

    // Finance event handlers for updating quests by objective type.
    private void HandleMoneyEarned(int amount) {
        UpdateQuestsByObjective(ObjectiveType.MoneyEarned, amount);
    }

    private void HandleMoneySpent(int amount) {
        UpdateQuestsByObjective(ObjectiveType.MoneySpent, amount);
    }
}
