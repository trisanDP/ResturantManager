using UnityEngine;
using System.Collections.Generic;

public class EmployeeUIManager : MonoBehaviour {
    #region UI References
    [Header("UI References")]
    public Transform candidateListParent; // Parent for UI list items (e.g., ScrollView content)
    public GameObject candidateListItemPrefab; // Candidate UI prefab
    #endregion

    #region Settings & References
    [Header("Candidate Settings")]
    public int numberOfCandidates = 5;

    [Header("References")]
    public EmployeeManager employeeManager; // Reference to spawn employees
    #endregion

    private List<CandidateEmployeeData> candidateList = new List<CandidateEmployeeData>();

    #region Unity Methods
    private void Start() {
        GenerateCandidateData();
        PopulateCandidateUI();
    }
    #endregion

    #region Private Methods
    // Generate random candidate data
    void GenerateCandidateData() {
        string[] possibleNames = { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace" };
        for(int i = 0; i < numberOfCandidates; i++) {
            CandidateEmployeeData candidate = new CandidateEmployeeData {
                employeeName = possibleNames[Random.Range(0, possibleNames.Length)],
                role = (EmployeeRole)Random.Range(0, System.Enum.GetValues(typeof(EmployeeRole)).Length),
                cooking = Random.Range(1, 6),
                cleaning = Random.Range(1, 6),
                serving = Random.Range(1, 6),
                social = Random.Range(1, 6)
            };
            candidateList.Add(candidate);
        }
    }

    // Populate the UI with candidate list items
    void PopulateCandidateUI() {
        foreach(CandidateEmployeeData candidate in candidateList) {
            GameObject itemGO = Instantiate(candidateListItemPrefab, candidateListParent);
            CandidateListItemUI itemUI = itemGO.GetComponent<CandidateListItemUI>();
            if(itemUI != null) {
                itemUI.Setup(candidate, OnHireCandidate);
            }
        }
    }

    // Callback for hire button press
    void OnHireCandidate(CandidateEmployeeData candidate) {
        employeeManager.SpawnCandidateEmployee(candidate);
        Debug.Log($"Hired: {candidate.employeeName}");
    }
    #endregion
}
