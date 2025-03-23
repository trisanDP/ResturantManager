using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : MonoBehaviour {
    public static InGameUIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel; // Main gameplay HUD

    [Header("UI Elements")]
    public TextMeshProUGUI gameStateText; // (Optional) For display/debug

    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged += HandleGameStateChanged;
        else
            Debug.Log("Not LInked");
    }

    private void OnDisable() {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState) {
        switch(newState) {
            case GameState.MainMenu:
            hudPanel?.SetActive(false);
            pausePanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
            if(gameStateText != null) gameStateText.text = "Main Menu";
            break;
            case GameState.Playing:
            hudPanel?.SetActive(true);
            pausePanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
            if(gameStateText != null) gameStateText.text = "Playing";
            break;
            case GameState.Paused:
            hudPanel?.SetActive(false);
            pausePanel?.SetActive(true);
            gameOverPanel?.SetActive(false);
            if(gameStateText != null) gameStateText.text = "Paused";
            break;
            case GameState.GameOver:
            hudPanel?.SetActive(false);
            pausePanel?.SetActive(false);
            gameOverPanel?.SetActive(true);
            if(gameStateText != null) gameStateText.text = "Game Over";
            break;
        }
    }
}
