using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    // Persistent settings (for example, subtitles, sound volume, etc.)
    public bool subtitlesEnabled = true;
    public float soundVolume = 1.0f;

    [SerializeField] private GameStateManager gameStateManager;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
        Initialize();
    }

    private void Initialize() {
        LinkScripts();
        // When in MainMenu scene, set state to MainMenu; otherwise, default to Playing.
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            gameStateManager.SetState(GameState.MainMenu);
        else
            gameStateManager.SetState(GameState.Playing);
    }

    private void LinkScripts() {
        if(gameStateManager == null)
            gameStateManager = FindFirstObjectByType<GameStateManager>();
    }

    // Expose GameStateManager if needed.
    public GameStateManager CurrentGameStateManager => gameStateManager;
}
