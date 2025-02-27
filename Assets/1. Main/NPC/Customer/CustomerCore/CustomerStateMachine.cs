using System;
using UnityEngine;

namespace RestaurantManagement {
    public class CustomerStateMachine {
        public enum State {
            MovingToWaitingPoint,
            WaitingForTable,
            MovingToTable,
            Ordering,
            WaitingForFood,
            Eating,
            Paying,
            Leaving
        }
        public State CurrentState { get; private set; }

        private float stateTimer;
        private Customer owner;
        private Func<bool> hasReachedDestination;
        private Action<Vector3> moveTo;

        public void Initialize(Customer owner, Func<bool> hasReachedDestination, Action<Vector3> moveTo) {
            this.owner = owner;
            this.hasReachedDestination = hasReachedDestination;
            this.moveTo = moveTo;
            CurrentState = State.MovingToWaitingPoint;
        }

        public void Update(float deltaTime) {
            switch(CurrentState) {
                case State.MovingToWaitingPoint:
                if(hasReachedDestination())
                    CurrentState = State.WaitingForTable;
                break;

                case State.WaitingForTable:
                if(owner.FindAndReserveTable()) {
                    moveTo(owner.AssignedTable.GetReservedChairPosition(owner));
                    CurrentState = State.MovingToTable;
                }
                break;

                case State.MovingToTable:
                if(hasReachedDestination()) {
                    owner.AssignSeatAtTable();
                    stateTimer = owner.orderDelay; // Start the ordering delay.
                    CurrentState = State.Ordering;
                }
                break;

                case State.Ordering:
                stateTimer -= deltaTime;
                if(stateTimer <= 0f) {
                    if(owner.TryPlaceOrder())
                        CurrentState = State.WaitingForFood;
                    else {
                        owner.LeaveRestaurant();
                        CurrentState = State.Leaving;
                    }
                }
                break;

                case State.WaitingForFood:
                // Waiting for external notification (NotifyFoodDelivered).
                break;

                case State.Eating:
                stateTimer -= deltaTime;
                if(stateTimer <= 0f) {
                    stateTimer = owner.paymentDelay;
                    CurrentState = State.Paying;
                }
                break;

                case State.Paying:
                stateTimer -= deltaTime;
                if(stateTimer <= 0f) {
                    owner.ProcessPayment();
                    owner.LeaveRestaurant();
                    CurrentState = State.Leaving;
                }
                break;

                case State.Leaving:
                if(hasReachedDestination()) {
                    owner.DestroySelf();
                }
                break;
            }
        }

        public void StartEating(float duration) {
            stateTimer = duration;
            CurrentState = State.Eating;
        }
    }
}
