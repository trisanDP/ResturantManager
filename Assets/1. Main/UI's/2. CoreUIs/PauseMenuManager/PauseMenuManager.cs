using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
    public GameObject pauseMenuPanel;

    [Header("Buttons")]
    public Button continueButton;
    public Button saveButton;
    public Button loadButton;
    public Button settingsButton;
    public Button quitButton;

    private void Start() {
        // Hide the pause menu panel by default.
        pauseMenuPanel?.SetActive(false);
        continueButton.onClick.AddListener(Btn_Continue);
        saveButton.onClick.AddListener(OnSaveButton);
        loadButton.onClick.AddListener(OnLoadButton);
        settingsButton.onClick.AddListener(OnSettingButton);
        quitButton.onClick.AddListener(OnQuitButton);
    }

    public void Btn_Continue() {
        // Resume the game by switching the state to Playing.
        Debug.Log("Called");
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Playing);
    }

    private void OnSaveButton() {
        Debug.Log("Save functionality not implemented.");
    }

    private void OnLoadButton() {
        Debug.Log("Load functionality not implemented.");
    }

    private void OnSettingButton() {
        Debug.Log("Settings functionality not implemented.");
    }

    private void OnQuitButton() {
        // Return to the MainMenu scene.
        SceneManager.LoadScene("MainMenu");
    }
}
