using UnityEngine;

public class PersistentParent : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
