using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour {
    public static CookingManager Instance { get; private set; }

    public event Action OnCookingUpdated; // Triggered when a new item is added or updated

    private List<CookingProcess> activeProcesses = new List<CookingProcess>();

    private void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartCooking(FoodBoxObject foodBox, CookingTable table, float duration) {
        CookingProcess process = new CookingProcess {
            FoodBox = foodBox,
            CookingTable = table,
            TimeRemaining = duration
        };
        activeProcesses.Add(process);

        OnCookingUpdated?.Invoke();
        StartCoroutine(CookingRoutine(process));
    }

    private IEnumerator CookingRoutine(CookingProcess process) {
        while(process.TimeRemaining > 0) {
            process.TimeRemaining -= Time.deltaTime;
            yield return null;
        }
        process.FoodBox.AdvanceCookingStage();
        activeProcesses.Remove(process);
        OnCookingUpdated?.Invoke();
    }

    public List<CookingProcess> GetActiveProcesses() {
        return new List<CookingProcess>(activeProcesses); // Return a copy for safety
    }
}

[Serializable]
public class CookingProcess {
    public FoodBoxObject FoodBox;
    public CookingTable CookingTable;
    public float TimeRemaining;
}
