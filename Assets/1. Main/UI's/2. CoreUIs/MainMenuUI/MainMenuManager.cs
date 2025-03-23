using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Buttons")]
    public Button startButton;
    public Button newGameButton;
    public Button loadButton;
    public Button settingsButton_Open;
    public Button settingsButton_Close;
    public Button quitButton;

    private void Start() {
        startButton.onClick.AddListener(StartGame);
        newGameButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(ShowLoadPlaceholder);
        settingsButton_Open.onClick.AddListener(ToggleSettings);
        settingsButton_Close.onClick.AddListener(ToggleSettings);
        quitButton.onClick.AddListener(QuitGame);

        CloseAllPannel();

    }
    void CloseAllPannel() {
        settingsPanel.SetActive(false);
    }
    private void StartGame() {
        // Load your gameplay scene (ensure it’s added to Build Settings).
        SceneManager.LoadScene("Playground");
    }

    private void ShowLoadPlaceholder() {
        Debug.Log("Load option not implemented.");
    }

    private void ToggleSettings() {
        if(settingsPanel != null)
            settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    private void QuitGame() {
        Application.Quit();
    }
}
