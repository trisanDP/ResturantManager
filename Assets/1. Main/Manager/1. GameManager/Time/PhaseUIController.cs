using RestaurantManagement;
using TMPro;
using UnityEngine;

public class PhaseUIController : MonoBehaviour {
    #region Fields
    [Header("UI Reference")]
    public TextMeshProUGUI phaseText; // Displays the current phase.
    #endregion

    #region Unity Methods
    private void OnEnable() {
        if(PhaseManager.Instance != null) {
            PhaseManager.Instance.OnPrepPhaseStarted += HandlePrepPhaseStarted;
            PhaseManager.Instance.OnPrepPhaseEnded += HandlePrepPhaseEnded;
            PhaseManager.Instance.OnRestaurantOpened += HandleRestaurantOpened;
            PhaseManager.Instance.OnRestaurantClosed += HandleRestaurantClosed;
            PhaseManager.Instance.OnSimulationCycleEnded += HandleSimulationCycleEnded;
        } else {
            Debug.LogWarning("PhaseManager is null.");
        }
    }

    private void OnDisable() {
        if(PhaseManager.Instance != null) {
            PhaseManager.Instance.OnPrepPhaseStarted -= HandlePrepPhaseStarted;
            PhaseManager.Instance.OnPrepPhaseEnded -= HandlePrepPhaseEnded;
            PhaseManager.Instance.OnRestaurantOpened -= HandleRestaurantOpened;
            PhaseManager.Instance.OnRestaurantClosed -= HandleRestaurantClosed;
            PhaseManager.Instance.OnSimulationCycleEnded -= HandleSimulationCycleEnded;
        }
    }
    #endregion

    #region Event Handlers
    private void HandlePrepPhaseStarted() {
        UpdatePhase("Prep Phase Started");
    }

    private void HandlePrepPhaseEnded() {
        UpdatePhase("Prep Phase Ended");
    }

    private void HandleRestaurantOpened() {
        UpdatePhase("Restaurant Open");
    }

    private void HandleRestaurantClosed() {
        UpdatePhase("Restaurant Closed");
    }

    private void HandleSimulationCycleEnded() {
        UpdatePhase("Simulation Cycle Ended");
    }
    #endregion

    #region Helper Methods
    private void UpdatePhase(string message) {
        Debug.Log("Phase Updated: " + message);
        if(phaseText != null) {
            phaseText.text = message;
        }
    }
    #endregion
}
