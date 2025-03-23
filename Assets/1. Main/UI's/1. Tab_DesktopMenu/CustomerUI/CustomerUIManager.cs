using UnityEngine;
using System.Collections.Generic;
using RestaurantManagement;

public class CustomerUIManager : MonoBehaviour {
    #region Fields
    public static CustomerUIManager Instance { get; private set; }

    [Header("UI Setup")]
    public Transform customerListContent;   // Parent container for UI elements (e.g., ScrollView content)
    public GameObject customerUIPrefab;       // Prefab for a single customer UI element

    private Dictionary<Customer, CustomerUIElement> customerUIElements = new Dictionary<Customer, CustomerUIElement>();
    #endregion

    #region Unity Methods
    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region Public Methods
    public void AddCustomerUI(Customer customer) {
        if(customerUIPrefab == null || customerListContent == null)
            return;
        GameObject uiObj = Instantiate(customerUIPrefab, customerListContent);
        CustomerUIElement uiElement = uiObj.GetComponent<CustomerUIElement>();
        if(uiElement != null) {
            uiElement.Initialize(customer);
            customerUIElements.Add(customer, uiElement);
        }
    }

    public void RemoveCustomerUI(Customer customer) {
        if(customerUIElements.ContainsKey(customer)) {
            Destroy(customerUIElements[customer].gameObject);
            customerUIElements.Remove(customer);
        }
    }

    public void UpdateCustomerUI(Customer customer) {
        if(customerUIElements.TryGetValue(customer, out CustomerUIElement uiElement))
            uiElement.UpdateUI();
    }
    #endregion
}
