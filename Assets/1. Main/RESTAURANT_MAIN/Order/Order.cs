using static RestaurantManagement.Table;

namespace RestaurantManagement {
    public class Order {
        public Customer Customer;
        public Table Table;
        public FoodItemData FoodItemData;
        public Seat Seat; // Assuming there's a Seat class for seating positions

        public enum OrderStatus { Pending, Served, Completed }
        public OrderStatus Status;

        public Order(Customer customer, Table table, Seat seat, FoodItemData dish) {
            this.Customer = customer;
            this.Table = table;
            this.Seat = seat;
            this.FoodItemData = dish;
            Status = OrderStatus.Pending;
        }

        public void MarkServed() {
            Status = OrderStatus.Served;
        }

        public void MarkCompleted() {
            Status = OrderStatus.Completed;
        }
    }
}
