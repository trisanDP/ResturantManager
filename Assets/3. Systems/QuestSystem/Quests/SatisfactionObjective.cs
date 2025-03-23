using System;

[Serializable]
public class QuestObjective : IQuestObjective {
    public string description;    // Brief description of the objective
    public int targetAmount;      // Amount required to complete
    public int currentAmount;     // Current progress
    public ObjectiveType objectiveType; // Type to categorize this objective

    public virtual bool IsCompleted() {
        return currentAmount >= targetAmount;
    }

    public virtual void UpdateProgress(int amount) {
        currentAmount += amount;
    }

    public virtual void ResetObjective() {
        currentAmount = 0;
    }
}

public enum ObjectiveType {
    MoneyEarned,
    MoneySpent,
    Upgrade,
    Decoration,
    // Extend with other types as needed.
}

public class SatisfactionObjective : QuestObjective {
    public float requiredSatisfaction;  // e.g., 0.8 for 80%
    public float currentSatisfaction;     // Current satisfaction value

    public void UpdateSatisfaction(float satisfaction) {
        currentSatisfaction = satisfaction;
    }

    public override bool IsCompleted() {
        return currentSatisfaction >= requiredSatisfaction;
    }

    public override void ResetObjective() {
        currentSatisfaction = 0f;
    }
}
