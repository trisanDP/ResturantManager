using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance { get; private set; }

    [Header("Settings Data Asset")]
    public GameSettingsData settingsData; // Assign via Inspector

    // Event broadcast when any setting changes.
    public event Action<GameSettingsData> OnSettingsChanged;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        } else {
            Destroy(gameObject);
            return;
        }
        ApplySettings();
    }

    // Apply all settings from the asset
    public void ApplySettings() {
        Application.targetFrameRate = settingsData.targetFrameRate;
        QualitySettings.vSyncCount = settingsData.vSyncEnabled ? 1 : 0;
        Screen.SetResolution(settingsData.screenWidth, settingsData.screenHeight, settingsData.fullScreen);
        // (Assume audio manager reads soundVolume, etc.)
        OnSettingsChanged?.Invoke(settingsData);
    }

    public void SetTargetFrameRate(int frameRate) {
        settingsData.targetFrameRate = frameRate;
        Application.targetFrameRate = frameRate;
        OnSettingsChanged?.Invoke(settingsData);
    }

    public void SetVSync(bool enabled) {
        settingsData.vSyncEnabled = enabled;
        QualitySettings.vSyncCount = enabled ? 1 : 0;
        OnSettingsChanged?.Invoke(settingsData);
    }

    public void SetResolution(int width, int height, bool fullScreen) {
        settingsData.screenWidth = width;
        settingsData.screenHeight = height;
        settingsData.fullScreen = fullScreen;
        Screen.SetResolution(width, height, fullScreen);
        OnSettingsChanged?.Invoke(settingsData);
    }

    public void SetSoundVolume(float volume) {
        settingsData.soundVolume = volume;
        OnSettingsChanged?.Invoke(settingsData);
    }

    public void SetSubtitlesEnabled(bool enabled) {
        settingsData.subtitlesEnabled = enabled;
        OnSettingsChanged?.Invoke(settingsData);
    }
}
