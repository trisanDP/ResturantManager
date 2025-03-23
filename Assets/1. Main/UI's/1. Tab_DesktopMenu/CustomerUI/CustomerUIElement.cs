using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RestaurantManagement;

public class CustomerUIElement : MonoBehaviour {
    #region Fields
    [Header("UI References")]
    public TextMeshProUGUI customerNameText; // Customer's name
    public TextMeshProUGUI statusText;       // Current status (e.g., Waiting, Seating)
    public TextMeshProUGUI tableText;        // Table info
    public TextMeshProUGUI orderText;        // Order details
    public Image customerImage;              // Customer image/icon (optional)

    private Customer trackedCustomer;        // Associated customer
    #endregion

    #region Public Methods
    public void Initialize(Customer customer) {
        trackedCustomer = customer;
        UpdateUI();
    }

    public void UpdateUI() {
        if(trackedCustomer == null)
            return;
        customerNameText.text = trackedCustomer.name;
        //statusText.text = trackedCustomer.GetStateDescription();
        tableText.text = trackedCustomer.AssignedTable != null ? trackedCustomer.AssignedTable.name : "None";
        //orderText.text = trackedCustomer.GetOrderDescription();
    }
    #endregion
}
