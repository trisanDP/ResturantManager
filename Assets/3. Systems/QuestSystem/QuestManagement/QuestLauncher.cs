using UnityEngine;
using RestaurantManagement;
using System.Collections.Generic;

public class QuestLauncher : MonoBehaviour {
    private QuestManager questManager;
    private FinanceManager financeManager;
    private bool quest3Launched = false;

    private void Start() {
        questManager = QuestManager.Instance;
        financeManager = RestaurantManager.Instance.FinanceManager;

        LaunchQuest1();
        questManager.OnQuestCompleted += HandleQuestCompleted;
    }

    private void OnDestroy() {
        questManager.OnQuestCompleted -= HandleQuestCompleted;
    }

    // Launch Quest 1: Earn $100.
    private void LaunchQuest1() {
        Quest quest1 = new Quest() {
            questID = "quest1",
            questName = "Earn $100",
            description = "Earn a total of $100 to boost your restaurant.",
            objectives = new List<QuestObjective>() {
                new QuestObjective() { description = "Earn $100", targetAmount = 100, objectiveType = ObjectiveType.MoneyEarned }
            },
            rewards = new List<QuestReward>() {
                new QuestReward() { rewardType = QuestReward.RewardType.Cash, amount = 50 }
            },
            status = Quest.QuestStatus.Inactive
        };
        questManager.ActivateQuest(quest1);
        //Debug.Log("Launched Quest 1");
    }

    // Launch Quest 2: Earn $500.
    private void LaunchQuest2() {
        Quest quest2 = new Quest() {
            questID = "quest2",
            questName = "Earn $500",
            description = "Earn a total of $500 to unlock new upgrades.",
            objectives = new List<QuestObjective>() {
                new QuestObjective() { description = "Earn $500", targetAmount = 500, objectiveType = ObjectiveType.MoneyEarned }
            },
            rewards = new List<QuestReward>() {
                new QuestReward() { rewardType = QuestReward.RewardType.Cash, amount = 200 }
            },
            status = Quest.QuestStatus.Inactive
        };
        questManager.ActivateQuest(quest2);
        Debug.Log("Launched Quest 2");
    }

    // Launch Quest 3: Spend $1000.
    private void LaunchQuest3() {
        Quest quest3 = new Quest() {
            questID = "quest3",
            questName = "Spend $1000",
            description = "Spend $1000 on upgrades to improve your restaurant.",
            objectives = new List<QuestObjective>() {
                new QuestObjective() { description = "Spend $1000", targetAmount = 1000, objectiveType = ObjectiveType.MoneySpent }
            },
            rewards = new List<QuestReward>() {
                new QuestReward() { rewardType = QuestReward.RewardType.Reputation, amount = 100 }
            },
            status = Quest.QuestStatus.Inactive
        };
        questManager.ActivateQuest(quest3);
        Debug.Log("Launched Quest 3");
    }

    // When a quest completes, launch next quests as needed.
    private void HandleQuestCompleted(Quest completedQuest) {
        Debug.Log("QuestLauncher: Quest Completed: " + completedQuest.questID);
        if(completedQuest.questID == "quest1") {
            LaunchQuest2();
        }
    }

    private void Update() {
        // Check for Quest 3 launch condition based on BusinessBalance.
        if(!quest3Launched && financeManager.BusinessBalance >= 1000) {
            quest3Launched = true;
            LaunchQuest3();
        }
    }
}
