using UnityEngine;
using System;

namespace RestaurantManagement {
    public enum RestaurantState {
        Open,
        Closed
    }

    public class RestaurantStateManager : MonoBehaviour {
        public static RestaurantStateManager Instance { get; private set; }
        public event Action<RestaurantState> OnStateChanged;

        private RestaurantState currentState;
        public RestaurantState CurrentState {
            get { return currentState; }
            private set {
                if(currentState != value) {
                    currentState = value;
                    OnStateChanged?.Invoke(currentState);
                }
            }
        }

        void Awake() {
            if(Instance == null) {
                Instance = this;
                // Default state is Closed (allows menu changes)
                CurrentState = RestaurantState.Closed;
            } else {
                Destroy(gameObject);
            }
        }

        // Change restaurant state
        public void SetState(RestaurantState newState) {
            CurrentState = newState;
        }
    }
}
