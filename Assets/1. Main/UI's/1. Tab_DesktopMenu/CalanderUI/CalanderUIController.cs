using UnityEngine;
using TMPro;

public class CalendarUIController : MonoBehaviour {
    #region Fields
    [Header("UI References")]
    public TextMeshProUGUI dateText;      // Displays day number.
    public TextMeshProUGUI monthText;     // Displays month.
    public TextMeshProUGUI yearText;      // Displays year.
    public TextMeshProUGUI dayOfWeekText; // Displays day of week.
    #endregion

    #region Unity Methods
    private void OnEnable() {
        if(TimeManager.Instance != null)
            TimeManager.Instance.OnDayChanged += UpdateCalendar;
        UpdateCalendar();
    }

    private void OnDisable() {
        if(TimeManager.Instance != null)
            TimeManager.Instance.OnDayChanged -= UpdateCalendar;
    }
    #endregion

    #region Helper Methods
    private void UpdateCalendar() {
        if(TimeManager.Instance == null) return;
        dateText.text = "Day " + TimeManager.Instance.currentDay;
        monthText.text = "Month " + TimeManager.Instance.currentMonth;
        yearText.text = TimeManager.Instance.currentYear.ToString();
        int dayIndex = (TimeManager.Instance.currentDay - 1) % 7;
        string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        dayOfWeekText.text = days[dayIndex];
    }
    #endregion
}
