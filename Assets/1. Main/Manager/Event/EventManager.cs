using System;
using System.Collections.Generic;
using System.Linq;

public static class EventManager {

    #region Input-Related Custom Events
    // Dictionary for input-related events
    private static readonly Dictionary<string, Action<bool>> inputEventDictionary = new Dictionary<string, Action<bool>>();

    // Subscribe a listener to an input event
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

    #region Order-Related Events
/*    public static event Action<TableOrder> OnOrderPlaced;
    public static event Action<TableOrder> OnOrderCompleted;
    public static event Action<TableOrder> OnBillRequested;

    public static void OrderPlaced(TableOrder order) {
        OnOrderPlaced?.Invoke(order);
    }

    public static void OrderCompleted(TableOrder order) {
        OnOrderCompleted?.Invoke(order);
    }*/

/*    public static void BillRequested(TableOrder order) {
        OnBillRequested?.Invoke(order);
    }*/
    #endregion
}
