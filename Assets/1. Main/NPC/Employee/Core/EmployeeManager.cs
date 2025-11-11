using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmployeeManager : MonoBehaviour {
    #region Fields & Properties
    [Header("Employee Prefab & Spawn Settings")]
    public GameObject employeePrefab;
    public Transform spawnPoint;

    [Header("Special Employees (ScriptableObjects)")]
    public List<EmployeeDataSO> specialEmployeeData;

    // Runtime list of employees
    public List<Employee> employeeList = new List<Employee>();
    #endregion

    #region Unity Methods
    private void Start() {
        foreach(EmployeeDataSO data in specialEmployeeData) {
            Employee newEmp = SpawnEmployeeFromData(data);
            employeeList.Add(newEmp);
        }

        // Optionally generate 5 random employees. //Test
/*        for(int i = 0; i < 5; i++) {
            Employee randomEmp = SpawnRandomEmployee();
            employeeList.Add(randomEmp);
        }*/
    }
    #endregion

    #region Public Methods
    // Spawn employee from ScriptableObject data.
    public Employee SpawnEmployeeFromData(EmployeeDataSO data) {
        GameObject empGO = Instantiate(employeePrefab, spawnPoint.position, Quaternion.identity);
        Employee emp = empGO.GetComponent<Employee>();
        if(emp != null) {
            emp.employeeName = data.employeeName;
            emp.role = data.role;
            emp.cooking = data.baseCooking;
            emp.cleaning = data.baseCleaning;
            emp.serving = data.baseServing;
            emp.social = data.baseSocial;

            // Set movement speed.
            EmployeeMovement move = emp.movement;
            if(move != null) {
                NavMeshAgent agent = move.agent;
                if(agent != null) {
                    agent.speed = data.movementSpeed * data.movementSpeedModifier;
                }
            }
        }
        return emp;
    }

/* Testing
    public Employee SpawnRandomEmployee() {
        GameObject empGO = Instantiate(employeePrefab, spawnPoint.position, Quaternion.identity);
        Employee emp = empGO.GetComponent<Employee>();
        if(emp != null) {
            string[] names = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            emp.employeeName = names[Random.Range(0, names.Length)];
            emp.role = (EmployeeRole)Random.Range(0, System.Enum.GetValues(typeof(EmployeeRole)).Length);
            emp.cooking = Random.Range(1, 6);
            emp.cleaning = Random.Range(1, 6);
            emp.serving = Random.Range(1, 6);
            emp.social = Random.Range(1, 6);

            // Set random movement speed.
            EmployeeMovement move = emp.movement;
            if(move != null) {
                NavMeshAgent agent = move.agent;
                if(agent != null) {
                    agent.speed = Random.Range(3f, 5f);
                }
            }
        }
        return emp;
    }
*/

    public Employee SpawnCandidateEmployee(CandidateEmployeeData candidateData) {
        GameObject empGO = Instantiate(employeePrefab, spawnPoint.position, Quaternion.identity);
        Employee emp = empGO.GetComponent<Employee>();
        if(emp != null) {
            emp.employeeName = candidateData.employeeName;
            emp.role = candidateData.role;
            emp.cooking = candidateData.cooking;
            emp.cleaning = candidateData.cleaning;
            emp.serving = candidateData.serving;
            emp.social = candidateData.social;
        }
        employeeList.Add(emp);
        return emp;
    }
    #endregion
}
