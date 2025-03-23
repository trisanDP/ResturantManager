using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour, IInteractable {
    public static SpawnManager Instance;

    [Header("Customer Spawner Settings")]
    public GameObject customerPrefab;
    public Transform spawnLocation;
    public Transform exitLocation; // Exit location for customers

    [Header("Spawn Timing")]
    public float baseSpawnRate = 5f;
    public bool autoSpawn = true;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if(autoSpawn) {
            StartCoroutine(SpawnCustomerRoutine());
        }
    }

    private IEnumerator SpawnCustomerRoutine() {
        while(true) {
            float waitTime = CalculateWaitTime();
            yield return new WaitForSeconds(waitTime);
            SpawnCustomer();
        }
    }

    private float CalculateWaitTime() {
        if(RestaurantManagement.RestaurantManager.Instance != null && RestaurantManagement.RestaurantManager.Instance.UseSpawnLogic) {  
            float popularityFactor = RestaurantManagement.RestaurantManager.Instance.Popularity;
            float marketingFactor = RestaurantManagement.RestaurantManager.Instance.MarketingLevel;
            float adjustedRate = baseSpawnRate / Mathf.Max(1f, (popularityFactor + marketingFactor) * 0.1f);
            return adjustedRate;
        }
        return baseSpawnRate;
    }

    private void SpawnCustomer() {
        if(customerPrefab == null || spawnLocation == null) {
            Debug.LogError("Customer Prefab or Spawn Location is not set.");
            return;
        }
        Instantiate(customerPrefab, spawnLocation.position, spawnLocation.rotation);
    }

    #region IInteractable Implementation
    public void OnFocusEnter() {
        // Optionally add UI feedback
    }
    public void OnFocusExit() {
        // Optionally remove UI feedback
    }
    public void Interact(BoxController controller) {
        SpawnCustomer();
    }
    #endregion
}
