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
        internal float stateTimer;
        private Customer owner;
        private Func<bool> hasReachedDestination;
        private Action<Vector3> moveTo;

        #region Initialization
        public void Initialize(Customer owner, Func<bool> hasReachedDestination, Action<Vector3> moveTo) {
            this.owner = owner;
            this.hasReachedDestination = hasReachedDestination;
            this.moveTo = moveTo;
            CurrentState = State.MovingToWaitingPoint;
        }
        #endregion

        #region Update Loop
        public void Update(float deltaTime) {
            switch(CurrentState) {
                case State.MovingToWaitingPoint:
                if(hasReachedDestination())
                    CurrentState = State.WaitingForTable;
                break;

                case State.WaitingForTable:
                if(owner.IsTableAvailable()) {
                    Table table = owner.FindAvailableTable();
                    if(table != null) {
                        moveTo(table.GetReservedSeatPosition(owner));
                        CurrentState = State.MovingToTable;
                    }
                }
                break;

                case State.MovingToTable:
                if(hasReachedDestination()) {
                    owner.AssignSeatAtTable();
                    stateTimer = owner.orderDelay;
                    CurrentState = State.Ordering;
                }
                break;

                case State.Ordering:
                // Optionally, trigger the talk animation at the start of ordering.
                if(owner.animator != null)
                    owner.animator.SetBool("IsTalking", true);
                stateTimer -= deltaTime;
                if(stateTimer <= 0f) {
                    if(owner.PlaceOrder())
                        CurrentState = State.WaitingForFood;
                    else {
                        owner.LeaveRestaurant();
                        CurrentState = State.Leaving;
                    }
                    // Reset talk parameter once done ordering
                    if(owner.animator != null)
                        owner.animator.SetBool("IsTalking", false);
                }
                break;


                case State.WaitingForFood:
                // External event (NotifyFoodDelivered) should trigger StartEating.
                break;

                case State.Eating:
                stateTimer -= deltaTime;
                if(stateTimer <= 0f) {
                    // Tell the customer to finish eating the food (destroy it)
                    owner.FinishEatingFood();
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
                if(hasReachedDestination())
                    owner.DestroySelf();
                break;
            }
        }
        #endregion

        #region State Transitions
        public void StartEating(float duration) {
            stateTimer = duration;
            CurrentState = State.Eating;
        }
        #endregion
    }
}
