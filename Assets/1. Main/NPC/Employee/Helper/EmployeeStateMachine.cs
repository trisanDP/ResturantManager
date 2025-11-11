using UnityEngine;
using UnityEngine.AI;

public class EmployeeStateManager {
    private Employee employee;
    private NavMeshAgent agent;
    private Animator animator;
    private EmployeeMovement movementHelper;

    private Vector3 targetDestination;
    private bool destinationSet = false;

    public void Initialize(Employee employee, NavMeshAgent agent, Animator animator) {
        this.employee = employee;
        this.agent = agent;
        this.animator = animator;
        movementHelper = employee.movement;
    }

    // Called by Employee.Update().
    public void UpdateState() {
/*        Debug.Log(employee.currentState);
        if(employee.currentTask==null) {
            Debug.Log("No task");
        }*/
        switch(employee.currentState) {
            case EmployeeState.Idle:
            // If no task, try to find one.
            if(employee.currentTask == null) {
                var task = TaskFinder.GetAvailableTask(employee.role);
                if(task != null && task.IsValidFor(employee)) {
                    employee.AssignTask(task);
                }
            }
            break;
            case EmployeeState.Moving:
            if(destinationSet) {
                movementHelper.MoveTo(targetDestination);
                if(movementHelper.IsAtDestination(targetDestination)) {
                    destinationSet = false;
                    ChangeState(EmployeeState.Working);
                }
            }
            break;
            case EmployeeState.Working:
            employee.workProgress += Time.deltaTime;
            if(employee.workProgress >= 3f) {
                employee.workProgress = 0f;
                employee.PerformRoleTask();
                employee.currentTask = null; // Task done.
                ChangeState(EmployeeState.Idle);
            }
            break;
            case EmployeeState.Carrying:
            if(destinationSet) {
                movementHelper.MoveTo(targetDestination);
                if(movementHelper.IsAtDestination(targetDestination)) {
                    destinationSet = false;
                    ChangeState(EmployeeState.Idle);
                }
            }
            break;
        }
    }

    public void ChangeState(EmployeeState newState) {
        employee.currentState = newState;
        switch(newState) {
            case EmployeeState.Idle:
            animator.SetTrigger("Idle");
            break;
            case EmployeeState.Moving:
            animator.SetTrigger("Walk");
            break;
            case EmployeeState.Working:
            animator.SetTrigger("Work");
            break;
            case EmployeeState.Carrying:
            animator.SetTrigger("Carry");
            break;
        }
    }


    public void SetDestination(Vector3 destination) {
        targetDestination = destination;
        destinationSet = true;
        ChangeState(EmployeeState.Moving);
    }


    public void StartCarrying(Vector3 destination) {
        targetDestination = destination;
        destinationSet = true;
        ChangeState(EmployeeState.Carrying);
    }
}
