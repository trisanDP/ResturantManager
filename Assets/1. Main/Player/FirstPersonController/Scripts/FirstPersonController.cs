using UnityEngine;

namespace Game_StarterAssets {
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour {
        #region Variables

        [Header("Player")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;

        [Header("Jump and Gravity")]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public float JumpTimeout = 0.1f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        private float _cinemachineTargetPitch;
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        #endregion

        #region Unity Methods

        private void Awake() {
            if(_mainCamera == null) {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start() {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            // Make sure the game is unpaused at scene start
            Time.timeScale = 1f;

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            EventManager.Subscribe("TabToggle", OnTabToggle);
        }



        private void OnDestroy() {
            EventManager.Unsubscribe("TabToggle", OnTabToggle);
        }

        private void Update() {
            if(!_input.tabMenuOpen) {
                JumpAndGravity();
                GroundedCheck();
                Move();
            }
        }

        private void LateUpdate() {
            // Only update camera rotation if neither the tab nor the pause menu is open
            if(!_input.tabMenuOpen && !_input.isPaused) {
                CameraRotation();
            }
        }

        private void GroundedCheck() {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }

        #endregion

        #region Input Handlers

        private void OnTabToggle(bool isTabOpen) {
            if(isTabOpen) {
                _input.move = Vector2.zero; // Stop movement
            }
        }

        #endregion

        #region Helper Methods

        private void CameraRotation() {
            if(_input.look.sqrMagnitude >= _threshold) {
                float deltaTimeMultiplier = 1.0f;
                _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        private void Move() {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if(_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if(currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset) {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            } else {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            if(_input.move != Vector2.zero) {
                inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void JumpAndGravity() {
            if(Grounded) {
                _fallTimeoutDelta = FallTimeout;

                if(_verticalVelocity < 0.0f) {
                    _verticalVelocity = -2f;
                }

                if(_input.jump && _jumpTimeoutDelta <= 0.0f) {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }

                if(_jumpTimeoutDelta >= 0.0f) {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            } else {
                _jumpTimeoutDelta = JumpTimeout;

                if(_fallTimeoutDelta >= 0.0f) {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                _input.jump = false;
            }

            if(_verticalVelocity < _terminalVelocity) {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
            if(lfAngle < -360f) lfAngle += 360f;
            if(lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        #endregion
    }
}
