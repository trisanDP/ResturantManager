using UnityEngine;
using System;
using System.Collections.Generic;

#region Quest Class
[Serializable]
public class Quest {
    public string questID;            // Unique quest identifier
    public string questName;          // Quest title
    public string description;        // Quest description
    public List<QuestObjective> objectives = new List<QuestObjective>();  // Objectives to complete
    public List<QuestReward> rewards = new List<QuestReward>();           // Rewards upon completion
    public QuestStatus status;        // Current quest status

    public enum QuestStatus { Inactive, Active, Completed, Failed }

    public void StartQuest() {
        status = QuestStatus.Active;
        ResetObjectives();
    }

    public bool CheckCompletion() {
        foreach(var obj in objectives) {
            if(!obj.IsCompleted())
                return false;
        }
        status = QuestStatus.Completed;
        return true;
    }

    public void ResetObjectives() {
        foreach(var obj in objectives)
            obj.ResetObjective();
    }
}

#endregion

#region IQuestObjective Interface
// Interface for quest objectives.
public interface IQuestObjective {
    bool IsCompleted();
    void UpdateProgress(int amount);
    void ResetObjective();
}

#endregion

// QuestReward.cs
#region QuestReward Class
[Serializable]
public class QuestReward {
    public enum RewardType { Cash, Reputation }
    public RewardType rewardType;   // Type of reward
    public decimal amount;          // Amount of reward

    public string GetRewardText() {
        return rewardType == RewardType.Cash
            ? "Cash: $" + amount
            : "Reputation: " + amount + " RP";
    }
}
#endregion
