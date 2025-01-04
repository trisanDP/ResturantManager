using UnityEngine;

public class TabUI : MonoBehaviour {
    public GameObject MainTabPanel;

    private void OnEnable() {
        EventManager.Subscribe("TabToggle", HandleTabToggle);
    }

    private void OnDisable() {
        EventManager.Unsubscribe("TabToggle", HandleTabToggle);
    }

    private void HandleTabToggle(bool isTabOpen) {
        MainTabPanel.SetActive(isTabOpen);
    }
}
