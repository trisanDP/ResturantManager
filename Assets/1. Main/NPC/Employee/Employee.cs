using UnityEngine;
using UnityEngine.AI;

#region Enums
public enum EmployeeState {
    Idle,
    Moving,
    Working,
    Carrying
}

public enum EmployeeRole {
    Cook,
    Waiter,
    KitchenHelper
}
#endregion


public class Employee : MonoBehaviour {
    #region Fields & Properties
    [Header("Employee Data")]
    public string employeeName = "Unnamed";
    public EmployeeRole role = EmployeeRole.Cook;

    // Base stats (range: 1 - 10)
    public int cooking = 1;
    public int cleaning = 1;
    public int serving = 1;
    public int social = 1;

    // Work progress simulation
    public float workProgress = 0f;

    // Current state (managed externally)
    public EmployeeState currentState = EmployeeState.Idle;

    public EmployeeStateManager stateManager { get; private set; }
    public EmployeeMovement movement { get; private set; }
    #endregion


    public EmployeeTask currentTask;  // Optionally, track the current task.

    public void AssignTask(EmployeeTask task) {
        currentTask = task;
        task.Execute(this);

    }

    private void Awake() {
        // Initialize the helper once with required components.
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();
        stateManager = new EmployeeStateManager();
        movement = new EmployeeMovement();
        movement.Initialize(agent);
        stateManager.Initialize(this, agent, animator);
    }
    private void Update() {
        // Delegate state updates to the helper class.
        stateManager.UpdateState();
    }


    #region Public Methods

    // Called when a work cycle completes to update stats.
    public void PerformRoleTask() {
        Debug.Log(role);
        switch(role) {
            case EmployeeRole.Cook:
            cooking = Mathf.Clamp(cooking + 1, 1, 10);
            break;
            case EmployeeRole.Waiter:
            serving = Mathf.Clamp(serving + 1, 1, 10);
            break;
            case EmployeeRole.KitchenHelper:
            cleaning = Mathf.Clamp(cleaning + 1, 1, 10);
            break;
        }
        Debug.Log($"{employeeName} performed a task as a {role}. Stats: Cooking {cooking}, Serving {serving}, Cleaning {cleaning}");
    }
    #endregion
}
