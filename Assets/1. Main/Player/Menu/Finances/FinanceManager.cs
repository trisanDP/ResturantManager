using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FinanceManager {
    #region Fields
    private decimal businessBalance = 0;
    private decimal personalBalance = 0;
    private List<Transaction> transactionHistory = new List<Transaction>();
    #endregion

    #region Events
    // Standard C# events
    public event Action OnBusinessBalanceChanged = delegate { };
    public event Action OnPersonalBalanceChanged = delegate { };
    public event Action OnTransactionHistoryChanged = delegate { };
    #endregion

    #region Properties
    public decimal BusinessBalance => businessBalance;
    public decimal PersonalBalance => personalBalance;
    public IReadOnlyList<Transaction> TransactionHistory => transactionHistory.AsReadOnly();
    #endregion

    #region Business Transactions
    public void AddBusinessIncome(decimal amount, string description = "Business Income") {
        businessBalance += amount;
        RecordTransaction(amount, TransactionType.BusinessIncome, description);
        OnBusinessBalanceChanged?.Invoke();
        // Fire finance event for quest tracking
        EventManager.MoneyEarned((int)amount);
    }

    public bool DeductBusinessExpense(decimal amount, string description = "Business Expense") {
        if(businessBalance >= amount) {
            businessBalance -= amount;
            RecordTransaction(amount, TransactionType.BusinessExpense, description);
            OnBusinessBalanceChanged?.Invoke();
            EventManager.MoneySpent((int)amount);
            return true;
        }
        Debug.LogError("FinanceManager: Insufficient business funds.");
        return false;
    }


    #endregion

    #region Personal Transactions
    public void AddPersonalIncome(decimal amount, string description = "Personal Income") {
        personalBalance += amount;
        RecordTransaction(amount, TransactionType.PersonalIncome, description);
        OnPersonalBalanceChanged?.Invoke();
    }

    public bool DeductPersonalExpense(decimal amount, string description = "Personal Expense") {
        if(personalBalance >= amount) {
            personalBalance -= amount;
            RecordTransaction(amount, TransactionType.PersonalExpense, description);
            OnPersonalBalanceChanged?.Invoke();
            return true;
        }
        Debug.LogError("FinanceManager: Insufficient personal funds.");
        return false;
    }
    #endregion

    #region Transfers
    public bool TransferToPersonal(decimal amount) {
        if(businessBalance >= amount) {
            businessBalance -= amount;
            personalBalance += amount;
            RecordTransaction(amount, TransactionType.TransferToPersonal, "Transfer to Personal");
            OnBusinessBalanceChanged?.Invoke();
            OnPersonalBalanceChanged?.Invoke();
            return true;
        }
        Debug.LogError("FinanceManager: Insufficient business funds for transfer.");
        return false;
    }

    public bool TransferToBusiness(decimal amount) {
        if(personalBalance >= amount) {
            personalBalance -= amount;
            businessBalance += amount;
            RecordTransaction(amount, TransactionType.TransferToBusiness, "Transfer to Business");
            OnBusinessBalanceChanged?.Invoke();
            OnPersonalBalanceChanged?.Invoke();
            return true;
        }
        Debug.LogError("FinanceManager: Insufficient personal funds for transfer.");
        return false;
    }
    #endregion

    #region Transaction Recording
    private void RecordTransaction(decimal amount, TransactionType type, string description) {
        transactionHistory.Add(new Transaction {
            Amount = amount,
            Type = type,
            Description = description,
            Timestamp = DateTime.Now
        });
        OnTransactionHistoryChanged?.Invoke();
    }
    #endregion

    #region Data Types
    [Serializable]
    public class Transaction {
        public decimal Amount;
        public TransactionType Type;
        public string Description;
        public DateTime Timestamp;
    }

    public enum TransactionType {
        BusinessIncome,
        BusinessExpense,
        PersonalIncome,
        PersonalExpense,
        TransferToPersonal,
        TransferToBusiness
    }
    #endregion
}
