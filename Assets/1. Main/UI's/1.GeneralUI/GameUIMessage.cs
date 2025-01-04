using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIMessage : MonoBehaviour
{
    #region Variable
    public GameObject OnScreenMessageGrp;
    [SerializeField] TextMeshProUGUI onScreenMessageTxt;

    #endregion

    public static GameUIMessage Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }else
            Destroy(Instance);
        DeactivateUI();
    }

    #region message display
    public void DisplayMessage(string message, int time) {
        StartCoroutine(DisplayMessageNow(message, 5));
    }
    public IEnumerator DisplayMessageNow(string message, int time) {
        OnScreenMessageGrp.SetActive(true);
        onScreenMessageTxt.enabled = true;
        onScreenMessageTxt.text = message;   // Display the message

        LogMessage(message);
        yield return new WaitForSeconds(time);  // Wait for the specified time
        onScreenMessageTxt.enabled = false;  // Hide the message
        OnScreenMessageGrp.SetActive(false);

    }

    void ActivateUI() {
        OnScreenMessageGrp.SetActive(true);
        Debug.Log("Hello");
    }
    void DeactivateUI() {
        OnScreenMessageGrp.SetActive(false);
        onScreenMessageTxt = null;
    }

    private void LogMessage(string message) {
        string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logEntry = timeStamp + ": " + message + "\n";
    }


    #endregion
}
