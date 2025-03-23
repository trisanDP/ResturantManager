using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using System;

public enum GameState {
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    // Broadcast game state changes for scene-specific UI to subscribe to.
    public event Action<GameState> OnStateChanged;

    private StarterAssetsInputs inputAssets;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
        // Subscribe to sceneLoaded event to update references when a new scene loads.
        SceneManager.sceneLoaded += OnSceneLoaded;
        inputAssets = FindFirstObjectByType<StarterAssetsInputs>();
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called each time a new scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("Called");
        inputAssets = FindFirstObjectByType<StarterAssetsInputs>();
        // Optionally reapply the current state to update the new input reference.
        CheckScene();
    }

    private void Start() {
        CheckScene();
    }
    void CheckScene() {
        // Set initial state based on active scene.
        if(SceneManager.GetActiveScene().name == "MainMenu")
            SetState(GameState.MainMenu);
        else
            SetState(GameState.Playing);
    }
    public void SetState(GameState newState) {
        if(CurrentState == newState)
            return;

        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(newState);
        OnStateChanged?.Invoke(newState);
    }

    private void EnterState(GameState state) {
        Debug.Log("Entering state: " + state);
        switch(state) {
            case GameState.MainMenu:
            Time.timeScale = 1f;
            break;
            case GameState.Playing:
            Time.timeScale = 1f;
            inputAssets?.SetPauseState(false);
            break;
            case GameState.Paused:
            Time.timeScale = 0f;
            inputAssets?.SetPauseState(true);
            break;
            case GameState.GameOver:
            Time.timeScale = 0f;
            break;
        }
    }

    private void ExitState(GameState state) {
        // Optional: add cleanup code here if needed.
    }

    public void TogglePause() {
        if(CurrentState == GameState.Playing)
            SetState(GameState.Paused);
        else if(CurrentState == GameState.Paused)
            SetState(GameState.Playing);
    }
}
