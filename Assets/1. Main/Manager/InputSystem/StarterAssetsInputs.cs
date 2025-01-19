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
            if(!tabMenuOpen) {
                move = context.ReadValue<Vector2>();
            }
        }

        public void OnLook(InputAction.CallbackContext context) {
            if(!tabMenuOpen && cursorInputForLook) {
                look = context.ReadValue<Vector2>();
            }
        }

        public void OnJump(InputAction.CallbackContext context) {
            if(!tabMenuOpen) {
                jump = context.ReadValueAsButton();
            }
        }

        public void OnSprint(InputAction.CallbackContext context) {
            if(!tabMenuOpen) {
                sprint = context.ReadValueAsButton();
            }
        }

        public void OnTab(InputAction.CallbackContext context) {
            if(context.performed) {
                tabMenuOpen = !tabMenuOpen;

                // Trigger Tab Toggle Event
                EventManager.Trigger("TabToggle", tabMenuOpen);

                // Lock or unlock the cursor
                SetCursorState(!tabMenuOpen);

                // Disable player input while tab is open
                cursorInputForLook = !tabMenuOpen;
                move = Vector2.zero;
            }
        }

        private void OnApplicationFocus(bool hasFocus) {
            if(!tabMenuOpen) {
                SetCursorState(cursorLocked);
            }
        }

        #endregion

        #region Helper Methods

        private void SetCursorState(bool newState) {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !newState;
        }

        #endregion
    }
}
