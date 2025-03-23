using System;
using System.Collections.Generic;
using System.Linq;

public static class EventManager {

    #region  Custom Events
    // Dictionary for input-related events
    private static readonly Dictionary<string, Action<bool>> inputEventDictionary = new Dictionary<string, Action<bool>>();


    public static void Subscribe(string eventName, Action<bool> listener) {
        if(!inputEventDictionary.ContainsKey(eventName))
            inputEventDictionary[eventName] = null;

        inputEventDictionary[eventName] += listener;
    }

    // Unsubscribe a listener from an input event
    public static void Unsubscribe(string eventName, Action<bool> listener) {
        if(inputEventDictionary.ContainsKey(eventName))
            inputEventDictionary[eventName] -= listener;
    }

    // Trigger an input event with a bool parameter
    public static void Trigger(string eventName, bool parameter) {
        if(inputEventDictionary.ContainsKey(eventName))
            inputEventDictionary[eventName]?.Invoke(parameter);
    }

    // Retrieve all subscribers for a specific input event
    public static List<Delegate> GetSubscribers(string eventName) {
        if(inputEventDictionary.ContainsKey(eventName) && inputEventDictionary[eventName] != null) {
            return inputEventDictionary[eventName].GetInvocationList().ToList();
        }
        return null; // No subscribers or event doesn't exist
    }
    #endregion

    #region Generic Events

    #region Game State Events
    public static event Action<bool> OnGameOver;
    public static event Action<bool> OnGamePaused;

    public static void GameOver(bool isWin) => OnGameOver?.Invoke(isWin);
    public static void GamePaused(bool isPaused) => OnGamePaused?.Invoke(isPaused);

    public static void TriggerGameOver(bool isWin) => OnGameOver?.Invoke(isWin);
    public static void TriggerGamePaused(bool isPaused) => OnGamePaused?.Invoke(isPaused);
    #endregion

    #region Finance Events
    public static event Action<int> OnMoneyEarned;
    public static event Action<int> OnMoneySpent;

    public static void MoneyEarned(int amount) => OnMoneyEarned?.Invoke(amount);
    public static void MoneySpent(int amount) => OnMoneySpent?.Invoke(amount);
    #endregion

    #region Quest Events

    public static event Action<string, int> OnQuestProgressUpdated;

    public static void UpdateQuestProgress(string questId, int progress)
        => OnQuestProgressUpdated?.Invoke(questId, progress);
    #endregion

    #endregion
}
