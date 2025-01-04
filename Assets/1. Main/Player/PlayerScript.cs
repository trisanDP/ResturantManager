using UnityEngine;

public class PlayerScript : MonoBehaviour, IUpdateObserver {
    private bool isTabOpen = false;

    public void ObservedUpdate() {
        ObserveInput();
    }

    void ObserveInput() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            isTabOpen = !isTabOpen;
            Debug.Log("Pressed");
            EventManager.Trigger("TabToggle", isTabOpen);
        }
    }



    void OnEnable() {
        UpdateManager.RegisterObserver(this);
    }
    void OnDestroy() {
        UpdateManager.UnregisterObserver(this);
    }
}
