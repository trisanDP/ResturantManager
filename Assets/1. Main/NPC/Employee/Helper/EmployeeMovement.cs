using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EmployeeMovement{
    public NavMeshAgent agent;


    #region Unity Methods
    
    public void Initialize(NavMeshAgent agnt) { 
        agent = agnt;
    }
    #endregion

    #region Public Methods
    // Sets the NavMesh destination.
    public void MoveTo(Vector3 destination) {
        if(agent.destination != destination) {
            agent.SetDestination(destination);
        }
    }

    // Checks if the agent has reached the destination.
    public bool IsAtDestination(Vector3 destination) {
        if(!agent.pathPending) {
            if(agent.remainingDistance <= agent.stoppingDistance) {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
}
