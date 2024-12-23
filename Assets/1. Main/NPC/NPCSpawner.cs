
using UnityEngine;

public class NPCSpawner : MonoBehaviour, IInteractable {
    [Header("NPC Spawner Settings")]
    public GameObject NPCPrefab;
    public Transform SpawnLocation;

    public void OnFocusEnter() {
        Debug.Log("Focusing on NPC Spawner");
    }

    public void OnFocusExit() {
        Debug.Log("Stopped focusing on NPC Spawner");
    }

    public void Interact(BoxController controller) {
        SpawnNPC();
    }

    private void SpawnNPC() {
        if(NPCPrefab == null || SpawnLocation == null) {
            Debug.LogError("NPC Prefab or Spawn Location is not set.");
            return;
        }

        GameObject npcObject = Instantiate(NPCPrefab, SpawnLocation.position, SpawnLocation.rotation);
        Debug.Log("NPC spawned successfully.");
    }
}
