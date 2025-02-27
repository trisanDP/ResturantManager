using UnityEngine;
using UnityEngine.AI;

namespace RestaurantManagement {
    public class CustomerMovementController {
        private NavMeshAgent agent;
        public float StoppingDistance { get; private set; }

        public CustomerMovementController(NavMeshAgent agent, float stoppingDistance) {
            this.agent = agent;
            StoppingDistance = stoppingDistance;
            agent.stoppingDistance = stoppingDistance;
            agent.autoBraking = false; // Disable auto-braking to reduce jitter.
        }

        public void MoveTo(Vector3 destination) {
            if(!agent.enabled)
                agent.enabled = true;
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public void StopMovement() {
            if(agent != null)
                agent.ResetPath();
        }

        public bool HasReachedDestination() {
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f;
        }

        public bool IsMoving {
            get {
                return agent.velocity.sqrMagnitude > 0.01f;
            }
        }
    }
}
