using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RePurpose_Models;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Models.Models.View;
using RePurpose_Models.Repositories.Interfaces;
using RePurpose_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Implementations
{

    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<IActionResult> GetAllTransaction()
        {
            var tran = await _unitOfWork.TransactionDb.GetAll()
                .ProjectTo<TransactionViewModel>(_mapper.ConfigurationProvider).ToListAsync()
                ;
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
        public async Task<Transaction?> getmytransaction1(Guid? id)
        {
            var tran = await _unitOfWork.TransactionDb.GetMany(product => product.WalletId.Equals(id) && product.Type == "PENDING")
                .FirstOrDefaultAsync();
            if (tran != null)
            {
                return tran;
            }
            return null;
        }
        public async Task<IActionResult> getmytransaction(Guid? id)
        {
            var tran = await _unitOfWork.TransactionDb.GetMany(product => product.WalletId.Equals(id))
                .ProjectTo<TransactionViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
        public Transaction getmytransaction2(long id)
        {
            var tran =  _unitOfWork.TransactionDb.GetMany(product => product.TransactionId == id)
                .FirstOrDefault();
            if (tran != null)
            {
                return tran;
            }
            return null;
        }
        public async Task<IActionResult> GetTransactionById(long id)
        {
            var tran = await _unitOfWork.TransactionDb.GetMany(product => product.TransactionId == id)
                .ProjectTo<TransactionViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
        public async Task<IActionResult> CreateTransaction(TransactionCreateModel transactionCreateModel, Guid id)
        {
            var walletId = await _unitOfWork.Wallet.FirstOrDefaultAsync(m => m.MemberId == id);
            if (walletId == null)
            {
                return new StatusCodeResult(400);
            }
            Transaction transaction = new Transaction
            {
                
                WalletId = walletId.WalletId,
                Amount = transactionCreateModel.Amount,
                Type = "PENDING",
                Timestamp = DateTime.Now
            };
            await _unitOfWork.TransactionDb.AddAsync(transaction);
            await _unitOfWork.SaveChanges();
           return await GetTransactionById(transaction.TransactionId);
           

        }
        public void UpdateOrderInfoInDatabase(Transaction transaction)
        {
            _unitOfWork.TransactionDb.Update(transaction);
        }
        /*public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var walletId = await _unitOfWork.Wallet.FirstOrDefaultAsync(m => m.MemberId == id);
            if (walletId == null)
            {
                return new StatusCodeResult(400);
            }
            var transaction =  _unitOfWork.TransactionDb.GetMany(m => m.WalletId == walletId.WalletId).ProjectTo<TransactionViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (transaction == null)
            {
                return new StatusCodeResult(400);
            }
            return new JsonResult(transaction);
        }*/

    }
}
