using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinanceManager : MonoBehaviour {

    #region Variables
    #region Fields
    [SerializeField] private decimal personalBalance = 1000m;
    [SerializeField] private decimal businessBalance = 5000m;

    private List<Transaction> transactionHistory = new();
    #endregion

    #region Events
    // UnityEvents for balance changes
    public UnityEvent OnPersonalBalanceChanged;
    public UnityEvent OnBusinessBalanceChanged;
    #endregion

    #region Enums
    public enum TransactionType { Income, Expenditure, TransferToBusiness, WithdrawalFromBusiness }
    #endregion

    #endregion

    #region Balance Management
    public decimal GetPersonalBalance() => personalBalance;
    public decimal GetBusinessBalance() => businessBalance;

    public void DepositToBusiness(decimal amount) {
        if(personalBalance >= amount) {
            personalBalance -= amount;
            businessBalance += amount;

            RecordTransaction(amount, TransactionType.TransferToBusiness);

            // Trigger balance update events
            OnPersonalBalanceChanged?.Invoke();
            OnBusinessBalanceChanged?.Invoke();
        } else {
            Debug.LogError("Insufficient personal funds.");
        }
    }

    public void WithdrawFromBusiness(decimal amount) {
        if(businessBalance >= amount) {
            businessBalance -= amount;
            personalBalance += amount;

            RecordTransaction(amount, TransactionType.WithdrawalFromBusiness);

            OnPersonalBalanceChanged?.Invoke();
            OnBusinessBalanceChanged?.Invoke();
        } else {
            Debug.LogError("Insufficient business funds.");
        }
    }
    #endregion

    public void GetLoan() {

    }


    #region Income and Expenditure
    public void AddIncome(decimal amount, string description = "Income") {
        businessBalance += amount;
        RecordTransaction(amount, TransactionType.Income, description);

        EventManager.Trigger("OnBusinessBalanceChanged", true);
    }

    public void AddExpenditure(decimal amount, string description = "Expenditure") {
        if(businessBalance >= amount) {
            businessBalance -= amount;
            RecordTransaction(amount, TransactionType.Expenditure, description);

            EventManager.Trigger("OnBusinessBalanceChanged", true);
        } else {
            Debug.LogError("Insufficient business funds.");
        }
    }
    #endregion

    #region Transaction History
    private void RecordTransaction(decimal amount, TransactionType type, string description = "") {
        transactionHistory.Add(new Transaction {
            Amount = amount,
            Type = type,
            Description = description,
            Timestamp = DateTime.Now
        });

        EventManager.Trigger("OnTransactionHistoryChanged", true);
    }

    public List<Transaction> GetTransactionHistory() => transactionHistory;
    #endregion

    #region Nested Classes
    [Serializable]
    public class Transaction {
        public decimal Amount;
        public TransactionType Type;
        public string Description;
        public DateTime Timestamp;
    }
    #endregion
}
