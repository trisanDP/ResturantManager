using UnityEngine;
using UnityEngine.AI;

namespace RestaurantManagement {
    public class CustomerMovementController {
        #region Fields
        private readonly NavMeshAgent agent;
        public float StoppingDistance { get; private set; } // Distance before stopping
        #endregion

        #region Constructor
        public CustomerMovementController(NavMeshAgent agent, float stoppingDistance) {
            if(agent == null) {
                Debug.LogError("NavMeshAgent is null!");
                return;
            }

            this.agent = agent;
            StoppingDistance = stoppingDistance;

            agent.stoppingDistance = stoppingDistance;
            agent.autoBraking = false; // Prevents unnecessary braking
            agent.updateRotation = true;
        }
        #endregion

        #region Movement Methods
        public void MoveTo(Vector3 destination) {
            if(agent == null) return;
            if(!agent.enabled) agent.enabled = true;

            agent.updateRotation = true; 
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public void StopMovement() {
            if(agent == null) return;
            agent.ResetPath();
            agent.isStopped = true;
        }

        public bool HasReachedDestination() {
            if(agent == null || !agent.enabled) return true;
            return !agent.pathPending && agent.remainingDistance <= StoppingDistance + 0.1f;
        }

        public bool IsMoving => agent != null && agent.enabled && agent.velocity.sqrMagnitude > 0.01f;
        #endregion

        #region Rotation Methods
        public void FaceTarget(Vector3 targetPosition) {
            if(agent == null || !agent.enabled) return;

            agent.updateRotation = false; // Disable auto rotation
            Vector3 direction = (targetPosition - agent.transform.position).normalized;
            if(direction.sqrMagnitude <= 0.001f) return; // Avoid unnecessary calculations

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
        #endregion
    }
}
