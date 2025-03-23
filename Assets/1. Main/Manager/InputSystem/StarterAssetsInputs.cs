using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets {
    public class StarterAssetsInputs : MonoBehaviour {
        #region Variables
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Interaction Settings")]
        public bool tabMenuOpen = false;
        public bool isPaused = false; // Flag for pause state

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private PlayerInput _playerInput;
        #endregion

        #region Unity Methods
        private void Start() {
            _playerInput = GetComponent<PlayerInput>();
        }

        public void OnMove(InputAction.CallbackContext context) {
            if (!tabMenuOpen && !isPaused)
                move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context) {
            if (!tabMenuOpen && !isPaused && cursorInputForLook)
                look = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context) {
            if (!tabMenuOpen && !isPaused)
                jump = context.ReadValueAsButton();
        }

        public void OnSprint(InputAction.CallbackContext context) {
            if (!tabMenuOpen && !isPaused)
                sprint = context.ReadValueAsButton();
        }

        public void OnTab(InputAction.CallbackContext context) {
            if (context.performed) {
                // Toggle the desktop overlay
                tabMenuOpen = !tabMenuOpen;
                EventManager.Trigger("TabToggle", tabMenuOpen);
                // Clear any residual input and update cursor state.
                look = Vector2.zero;
                SetCursorState(!tabMenuOpen);
                cursorInputForLook = !tabMenuOpen;
                move = Vector2.zero;
            }
        }

        public void OnPause(InputAction.CallbackContext context) {
            if (context.performed) {
                // If the desktop (tab) is open, close it instead of pausing.
                if (tabMenuOpen) {
                    CloseDesktop();
                    return;
                }
                // Otherwise, toggle game pause via GameStateManager.
                if (GameStateManager.Instance != null)
                    GameStateManager.Instance.TogglePause();

                isPaused = (GameStateManager.Instance.CurrentState == GameState.Paused);
                look = Vector2.zero;
                SetCursorState(!isPaused);
                cursorInputForLook = !isPaused;  
                move = Vector2.zero;
            }
        }

        private void OnApplicationFocus(bool hasFocus) {
            if (!tabMenuOpen && !isPaused)
                SetCursorState(cursorLocked);
        }
        #endregion

        #region Helper Methods
        private void SetCursorState(bool newState) {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !newState;
        }

        // This method is used to close the desktop overlay.
        public void CloseDesktop() {
            tabMenuOpen = false;
            EventManager.Trigger("TabToggle", false);
            // Restore normal cursor state.
            SetCursorState(cursorLocked);
            cursorInputForLook = true;
            // Optionally reset inputs.
            look = Vector2.zero;
            move = Vector2.zero;
        }

        // Public method to update pause state from GameStateManager.
        public void SetPauseState(bool paused) {
            isPaused = paused;
            SetCursorState(!paused);
            cursorInputForLook = !paused;
            look = Vector2.zero;
            move = Vector2.zero;
        }
        #endregion
    }
}
