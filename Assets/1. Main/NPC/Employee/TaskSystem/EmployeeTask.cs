// EmployeeTask.cs
#region EmployeeTask Base
public abstract class EmployeeTask {
    public enum TaskType { Cooking }
    public abstract TaskType Type { get; }
    public abstract bool IsValidFor(Employee employee);
    public abstract void Execute(Employee employee);
}
#endregion
