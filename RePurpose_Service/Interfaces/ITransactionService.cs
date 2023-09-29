using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Interfaces
{
    public interface ITransactionService
    {
        Task<IActionResult> CreateTransaction(TransactionCreateModel transactionCreateModel, Guid id);
        Task<IActionResult> GetTransactionById(long id);
        Task<IActionResult> GetAllTransaction();
        Task<IActionResult> getmytransaction(Guid? id);
        Task<Transaction?> getmytransaction1(Guid? id);
        void UpdateOrderInfoInDatabase(Transaction transaction);
    }
}
