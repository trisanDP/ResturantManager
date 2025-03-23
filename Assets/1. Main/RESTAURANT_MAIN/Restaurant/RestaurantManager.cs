using UnityEngine;

namespace RestaurantManagement {
    public class RestaurantManager : MonoBehaviour {
        public static RestaurantManager Instance { get; private set; }

        #region Manager Instances
        [Header("Manager Instances (Initialized in Code)")]
        public TableManager TableManager { get; private set; }
        public CustomerManager CustomerManager { get; private set; }
        public OrderManager OrderManager { get; private set; }
        public FinanceManager FinanceManager;
        public MenuBookManager MenuManager { get; private set; }  // Renamed to MenuBookManager
        public RestaurantStateManager RestaurantStateManager { get; private set; }
        #endregion

        #region Restaurant & Shop Settings
        [Header("Restaurant Settings")]
        public string RestaurantName;
        public Transform EntrancePoint;
        public Transform ExitPoint;
        public Transform KitchenPosition;
        public Transform WaitPoint;
        public float Popularity;
        public float MarketingLevel;
        public bool UseSpawnLogic;

        [Header("Shop Settings")]
        public GameObject FoodBoxPrefab;
        public Transform ShopSpawnPoint;
        #endregion

        #region Database
        [Header("Database")]
        public ItemDatabase itemDatabase; // Reference to the ItemDatabase asset
        #endregion

        #region Unity Methods
        private void Awake() {
            if(Instance == null) {
                Instance = this;
                InitializeManagers();
            } else {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Initialization
        void InitializeManagers() {
            TableManager = new TableManager();
            CustomerManager = new CustomerManager();
            OrderManager = new OrderManager();
            FinanceManager = new FinanceManager();  // Now created with new
            MenuManager = new MenuBookManager();      // Create a new instance of MenuBookManager
            if(itemDatabase != null)
                MenuManager.Initialize(itemDatabase);
            // Initialize other managers as needed.
        }
        #endregion
    }
}
