using NLog;
using SatisfactorySaveParser.Save;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SatisfactorySaveEditor.Service.Undo
{
    public class UndoService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<TransactionalFGSaveSession> _transactions = new List<TransactionalFGSaveSession>();

        public TransactionalFGSaveSession CreateTransaction(string name, FGSaveSession saveSession)
        {
            var isLastOpen = _transactions.LastOrDefault()?.IsActive ?? false;
            if (isLastOpen) throw new InvalidOperationException("Cannot start a new transaction without finishing the previous one before");

            var transaction = new TransactionalFGSaveSession(name, saveSession);
            _transactions.Add(transaction);

            return transaction;
        }

        public void UndoLastTransaction()
        {
            var isLastOpen = _transactions.LastOrDefault()?.IsActive ?? false;
            if (isLastOpen) throw new InvalidOperationException("Cannot undo transaction without finishing it before");

            _transactions.LastOrDefault()?.Undo();
        }

        public void ClearTransactions()
        {
            var isLastOpen = _transactions.LastOrDefault()?.IsActive ?? false;
            if (isLastOpen) _logger.Warn("Attempted to clear transaction history with an open transaction, the save session may be in an inconsistent state");

            _transactions.Clear();
        }
    }
}
