using UnityEngine;
using UnityEngine.AI;

namespace RestaurantManagement {
    // Helper to manage table reservation, payment, and leaving.
    public class CustomerSeatingHandler {
        private Customer customer;
        public CustomerSeatingHandler(Customer customer) {
            this.customer = customer;
        }

        public bool ReserveTable(Table table) {
            bool reserved = table.ReserveSeat(customer);
            if(reserved)
                customer.SetAssignedTable(table);
            return reserved;
        }



        public void LeaveRestaurant() {
            Table table = customer.AssignedTable;
            if(table != null) {
                table.RemoveCustomer(customer);
                RestaurantManager.Instance.TableManager.NotifyTableAvailabilityChanged(table);
            }
            customer.SetAssignedTable(null);
            customer.UnfreezeMovementAndRotation();
            NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
            if(agent != null && !agent.enabled)
                agent.enabled = true;

            if(SpawnManager.Instance?.exitLocation != null)
                customer.MoveTo(SpawnManager.Instance.exitLocation.position);
            else if(RestaurantManager.Instance?.ExitPoint != null)
                customer.MoveTo(RestaurantManager.Instance.ExitPoint.position);
            else
                Debug.LogError("No exit location defined!");
        }
    }
}
