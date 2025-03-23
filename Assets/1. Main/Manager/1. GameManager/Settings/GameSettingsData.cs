using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Settings/Game Settings Data")]
public class GameSettingsData : ScriptableObject {
    [Header("Display Settings")]
    public int targetFrameRate = 120;
    public bool vSyncEnabled = false;
    public int screenWidth = 1920;
    public int screenHeight = 1080;
    public bool fullScreen = true;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 1.0f;

    [Header("Gameplay Settings")]
    public bool subtitlesEnabled = true;
}
