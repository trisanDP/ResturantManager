using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUIController : MonoBehaviour {
    [Header("UI Controls")]
    public Slider frameRateSlider;
    public TextMeshProUGUI frameRateText; // Reference from the Inspector
    public Toggle vSyncToggle;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;



    private void Start() {
        // Initialize UI controls from the current settings.
        if(SettingsManager.Instance != null) {
            frameRateSlider.value = SettingsManager.Instance.settingsData.targetFrameRate;
            vSyncToggle.isOn = SettingsManager.Instance.settingsData.vSyncEnabled;
            fullscreenToggle.isOn = SettingsManager.Instance.settingsData.fullScreen;


            // Populate resolution dropdown (hardcoded for example)
            resolutionDropdown.ClearOptions();
            var options = new System.Collections.Generic.List<string> { "1920x1080", "1600x900", "1280x720" };
            resolutionDropdown.AddOptions(options);
            string currentRes = SettingsManager.Instance.settingsData.screenWidth + "x" + SettingsManager.Instance.settingsData.screenHeight;
            int index = options.IndexOf(currentRes);
            resolutionDropdown.value = index >= 0 ? index : 0;
        }

        // Add listeners programmatically
        frameRateSlider.onValueChanged.AddListener(delegate { OnFrameRateChanged(); });
        vSyncToggle.onValueChanged.AddListener(delegate { OnVSyncChanged(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChanged(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenChanged(); });

    }


    // These methods are called via the UI controls' OnValueChanged events.
    public void OnFrameRateChanged() {
        int newFrameRate = (int)frameRateSlider.value;
        SettingsManager.Instance.SetTargetFrameRate(newFrameRate);
        if(frameRateText != null)
            frameRateText.text = newFrameRate.ToString();
    }

    public void OnVSyncChanged() {
        SettingsManager.Instance.SetVSync(vSyncToggle.isOn);
    }

    public void OnResolutionChanged() {
        string option = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] parts = option.Split('x');
        if(parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height)) {
            SettingsManager.Instance.SetResolution(width, height, fullscreenToggle.isOn);
        }
    }

    public void OnFullscreenChanged() {
        // Update resolution using the current resolution values and fullscreen toggle.
        SettingsManager.Instance.SetResolution(
            SettingsManager.Instance.settingsData.screenWidth,
            SettingsManager.Instance.settingsData.screenHeight,
            fullscreenToggle.isOn);
    }

    public void BTN_ApplyChange() {
        SettingsManager.Instance.ApplySettings();
    }
}
