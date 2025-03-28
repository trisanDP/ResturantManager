// CookingTask.cs
#region CookingTask
using RestaurantManagement;

public class CookingTask : EmployeeTask {
    public Order OrderData;
    public Shelf TargetShelf;
    public CookingStation TargetStation;

    public override TaskType Type => TaskType.Cooking;
    public override bool IsValidFor(Employee employee) {
        return employee.role == EmployeeRole.Cook;
    }

    public override void Execute(Employee employee) {
        // 1. Move to shelf to pick up raw food (box).
        employee.stateManager.SetDestination(TargetShelf.transform.position);
        // After arrival, you'll want to proceed to cooking station, etc.
        // This can be done by storing a 'step index' or hooking into arrival logic.
    }
}
#endregion
