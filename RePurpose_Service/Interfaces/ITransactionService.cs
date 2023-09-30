using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;

namespace RePurpose_Service.Interfaces
{
    public interface ITransactionService
    {
        Task<IActionResult> CreateTransaction(TransactionCreateModel transactionCreateModel, Guid id);
        Task<IActionResult> GetTransactionById(long id);
        Task<IActionResult> GetAllTransaction();
        Task<IActionResult> getmytransaction(Guid? id);
        Task<Transaction?> getmytransaction1(Guid? id);
        Task<Transaction> getmytransaction2(long id);
        Task UpdateOrderInfoInDatabase(Transaction transaction);
    }
}
