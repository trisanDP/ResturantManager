using System;
using UnityEngine;

namespace RestaurantManagement {
    public class PhaseManager : MonoBehaviour {
        public static PhaseManager Instance { get; private set; }

        #region Fields
        [Header("Phase Settings")]
        public int prepPhaseStartHour = 5;   // Prep phase starts at 5 AM.
        public int prepPhaseEndHour = 9;     // Ends at 9 AM.
        public int openPhaseEndHour = 18;    // Restaurant closes at 6 PM.
        public int activeSimulationDays = 7; // Active gameplay days per cycle.
        #endregion

        #region Events
        public event Action OnPrepPhaseStarted;
        public event Action OnPrepPhaseEnded;
        public event Action OnRestaurantOpened;
        public event Action OnRestaurantClosed;
        public event Action OnSimulationCycleEnded;
        #endregion

        #region Unity Methods
        private void Awake() {
            if(Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable() {
            if(TimeManager.Instance != null) {
                TimeManager.Instance.OnHourChanged += CheckPhase;
                TimeManager.Instance.OnDayChanged += OnDayChanged;
                // Immediately check the phase on enabling.
                CheckPhase(TimeManager.Instance.GetCurrentHour());
            }
        }

        private void OnDisable() {
            if(TimeManager.Instance != null) {
                TimeManager.Instance.OnHourChanged -= CheckPhase;
                TimeManager.Instance.OnDayChanged -= OnDayChanged;
            }
        }
        #endregion

        #region Phase Check Methods
        private void CheckPhase(int hour) {
            // Debug.Log("Checking phase at hour: " + hour);
            if(hour == prepPhaseStartHour)
                OnPrepPhaseStarted?.Invoke();
            if(hour == prepPhaseEndHour) {
                OnPrepPhaseEnded?.Invoke();
                OnRestaurantOpened?.Invoke();
            }
            if(hour == openPhaseEndHour)
                OnRestaurantClosed?.Invoke();
        }

        private void OnDayChanged() {
            if(TimeManager.Instance.currentDay > activeSimulationDays) {
                OnSimulationCycleEnded?.Invoke();
                Debug.Log("Simulation cycle ended. Calculate earnings and skip remaining days.");
            }
        }
        #endregion
    }
}
