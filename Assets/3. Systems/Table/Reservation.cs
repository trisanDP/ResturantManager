using System.Collections.Generic;
using RestaurantManagement; // For access to Customer and Table.Seat

namespace RestaurantManagement.Reservations {
    public class Reservation {
        private Dictionary<Customer, Table.Seat> reservedSeats = new Dictionary<Customer, Table.Seat>();

        public void Reserve(Customer customer, Table.Seat seat) {
            if(!reservedSeats.ContainsKey(customer)) {
                reservedSeats.Add(customer, seat);
            }
        }

        public Table.Seat GetReservedSeat(Customer customer) {
            reservedSeats.TryGetValue(customer, out Table.Seat seat);
            return seat;
        }

        public void ConfirmReservation(Customer customer) {
            if(reservedSeats.ContainsKey(customer))
                reservedSeats.Remove(customer);
        }
    }
}
