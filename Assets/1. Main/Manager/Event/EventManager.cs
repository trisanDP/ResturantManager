using System;
using System.Collections.Generic;

public static class EventManager {
    private static readonly Dictionary<string, Action<bool>> eventDictionary = new Dictionary<string, Action<bool>>();

    public static void Subscribe(string eventName, Action<bool> listener) {
        if(!eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = null;

        eventDictionary[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<bool> listener) {
        if(eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] -= listener;
    }

    public static void Trigger(string eventName, bool parameter) {
        if(eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName]?.Invoke(parameter);
    }
}
