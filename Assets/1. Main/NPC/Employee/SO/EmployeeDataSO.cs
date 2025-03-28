using UnityEngine;

[CreateAssetMenu(fileName = "EmployeeData", menuName = "Restaurant/Employee Data")]
public class EmployeeDataSO : ScriptableObject {
    #region Data Fields
    [Header("Basic Info")]
    public string employeeName;
    public EmployeeRole role;

    [Header("Base Stats (1 - 10)")]
    public int baseCooking = 1;
    public int baseCleaning = 1;
    public int baseServing = 1;
    public int baseSocial = 1;

    [Header("Movement & Work Settings")]
    public float movementSpeed = 3.5f;
    public float workCycleTime = 3f;

    [Header("Special Modifiers")]
    public float cookingSpeedModifier = 1.0f;
    public float cleaningSpeedModifier = 1.0f;
    public float movementSpeedModifier = 1.0f;
    #endregion
}
