using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets {
    public class StarterAssetsInputs : MonoBehaviour {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Interaction Settings")]
        public bool onInteract;
        public bool tabMenuOpen = false; // Tracks the Tab menu state

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value) {
            if(!tabMenuOpen) // Ignore movement input when the tab menu is open
            {
                MoveInput(value.Get<Vector2>());
            }
        }

        public void OnLook(InputValue value) {
            if(cursorInputForLook && !tabMenuOpen) // Ignore look input when the tab menu is open
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value) {
            if(!tabMenuOpen) // Ignore jump input when the tab menu is open
            {
                JumpInput(value.isPressed);
            }
        }

        public void OnSprint(InputValue value) {
            if(!tabMenuOpen) // Ignore sprint input when the tab menu is open
            {
                SprintInput(value.isPressed);
            }
        }

        public void OnTab(InputValue value) {
            if(value.isPressed) {
                tabMenuOpen = !tabMenuOpen;
                SetCursorState(!tabMenuOpen); // Lock/unlock cursor based on menu state
                cursorInputForLook = !tabMenuOpen; // Disable camera control when menu is open
            }
        }
#endif

        public void MoveInput(Vector2 newMoveDirection) {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection) {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState) {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState) {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus) {
            if(!tabMenuOpen) // Only lock the cursor if the tab menu is not open
            {
                SetCursorState(cursorLocked);
            }
        }

        private void SetCursorState(bool newState) {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !newState;
        }
    }
}
