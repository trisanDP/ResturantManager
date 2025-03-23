using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour {
    public static TimeManager Instance { get; private set; }

    #region Fields & Properties
    [Header("Time Settings")]
    public float simulationSpeed = 60f; // 1 real second = 60 in-game seconds.
    public int startHour = 5;           // Starting at 5 AM.
    public int startDay = 1;
    public int startMonth = 1;
    public int startYear = 2023;
    public int startDayOfWeek = 0;        // 0 = Sunday, 1 = Monday, etc.

    public float currentTimeInSeconds { get; private set; } // Seconds into current day.
    public int currentDay { get; private set; }
    public int currentMonth { get; private set; }
    public int currentYear { get; private set; }
    public int currentDayOfWeek { get; private set; } // 0 = Sunday, 1 = Monday, etc.
    public const int secondsPerDay = 86400; // 24 * 3600 seconds.
    #endregion

    #region Events
    public event Action<float> OnTimeChanged; // Called each update with currentTimeInSeconds.
    public event Action<int> OnHourChanged;   // Called when the hour changes.
    public event Action OnDayChanged;         // Called when a day finishes.
    #endregion

    #region Unity Methods
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        currentTimeInSeconds = startHour * 3600;
        currentDay = startDay;
        currentMonth = startMonth;
        currentYear = startYear;
        currentDayOfWeek = startDayOfWeek;

        // Fire an initial hour change event for the starting hour.
        OnHourChanged?.Invoke(GetCurrentHour());
    }


    private void Update() {
        float delta = Time.deltaTime * simulationSpeed;
        float previousTime = currentTimeInSeconds;
        currentTimeInSeconds += delta;

        if(currentTimeInSeconds >= secondsPerDay) {
            currentTimeInSeconds -= secondsPerDay;
            AdvanceDay();
        }

        OnTimeChanged?.Invoke(currentTimeInSeconds);

        int previousHour = Mathf.FloorToInt(previousTime / 3600);
        int currentHour = Mathf.FloorToInt(currentTimeInSeconds / 3600);
        if(currentHour != previousHour)
            OnHourChanged?.Invoke(currentHour);
    }
    #endregion

    #region Helper Methods
    private void AdvanceDay() {
        currentDay++;
        currentDayOfWeek = (currentDayOfWeek + 1) % 7;
        // Simplistic month handling: assume 30 days per month.
        if(currentDay > 30) {
            currentDay = 1;
            currentMonth++;
            if(currentMonth > 12) {
                currentMonth = 1;
                currentYear++;
            }
        }
        OnDayChanged?.Invoke();
    }

    // Returns a formatted date string, e.g., "Sun, Jan 1, 2023"
    public string GetFormattedDate() {
        string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        string dayName = dayNames[currentDayOfWeek];
        // Ensure month index is in range (months are 1-based in our settings)
        string monthName = monthNames[Mathf.Clamp(currentMonth - 1, 0, monthNames.Length - 1)];
        return $"{dayName}, {currentYear} {monthName} {currentDay}";
    }

    public int GetCurrentHour() {
        return Mathf.FloorToInt(currentTimeInSeconds / 3600);
    }
    #endregion
}
