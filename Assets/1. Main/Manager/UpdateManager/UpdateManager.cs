using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour {
     
    private static List<IUpdateObserver> _observers = new();
    private static List<IUpdateObserver> _pendingObservers = new();
    private static int _currentIndex;

    // Update is called once per frame
    void Update() {
        for(_currentIndex = _observers.Count -1; _currentIndex >=0; _currentIndex--) {
            _observers[_currentIndex].ObservedUpdate();
        }
        _observers.AddRange(_pendingObservers);
        _pendingObservers.Clear();
    }

    public static void RegisterObserver(IUpdateObserver observer) {
        _pendingObservers.Add(observer);
    }
    public static void UnregisterObserver(IUpdateObserver observer) {
        _pendingObservers.Remove(observer);
        _currentIndex--;
    }
}