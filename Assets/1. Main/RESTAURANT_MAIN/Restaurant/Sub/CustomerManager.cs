using System.Collections.Generic;

namespace RestaurantManagement {
    public class CustomerManager {
        private List<Customer> customers = new List<Customer>();

        public void RegisterCustomer(Customer customer) {
            if(!customers.Contains(customer)) {
                customers.Add(customer);
                CustomerUIManager.Instance?.AddCustomerUI(customer);
            }
        }

        public void UnregisterCustomer(Customer customer) {
            customers.Remove(customer);
            CustomerUIManager.Instance?.RemoveCustomerUI(customer);
        }
    }
}
