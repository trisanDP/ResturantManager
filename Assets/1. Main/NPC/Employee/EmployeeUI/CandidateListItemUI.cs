using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

#region Candidate Data Struct
public struct CandidateEmployeeData {
    public string employeeName;
    public EmployeeRole role;
    public int cooking;
    public int cleaning;
    public int serving;
    public int social;
}
#endregion

public class CandidateListItemUI : MonoBehaviour {
    #region UI References
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI roleText;
    public TextMeshProUGUI statsText;
    public Button hireButton;
    #endregion

    private CandidateEmployeeData candidateData;

    #region Public Methods
    // Setup UI item with candidate data and callback for hire button
    public void Setup(CandidateEmployeeData data, Action<CandidateEmployeeData> hireCallback) {
        candidateData = data;
        nameText.text = candidateData.employeeName;
        roleText.text = candidateData.role.ToString();
        statsText.text = $"Cooking: {candidateData.cooking}  Cleaning: {candidateData.cleaning}  Serving: {candidateData.serving}  Social: {candidateData.social}";
        hireButton.onClick.RemoveAllListeners();
        hireButton.onClick.AddListener(() => hireCallback(candidateData));
    }
    #endregion
}
