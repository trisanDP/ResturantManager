/*using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour {
    #region Singleton
    public static CookingManager Instance { get; private set; }
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Fields
    private readonly List<CookingSlot> _activeCookingSlots = new();
    #endregion

    #region Public Methods
    public void RegisterSlot(CookingSlot slot) {
        if(!_activeCookingSlots.Contains(slot)) {
            _activeCookingSlots.Add(slot);
        }
    }

    public void UnregisterSlot(CookingSlot slot) {
        if(_activeCookingSlots.Contains(slot)) {
            _activeCookingSlots.Remove(slot);
        }
    }

    private void Update() {
        foreach(var slot in _activeCookingSlots) {
            if(!slot.IsAvailable) {
                slot.UpdateTime(Time.deltaTime);
                float progress = 1f - (slot.RemainingTime / slot.TotalCookingTime);
               *//* CookingUIManager.Instance?.UpdateProgressBar(slot.FoodBox, progress);*//*
            }
        }
    }
    #endregion
}
*/