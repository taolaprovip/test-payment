using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RePurpose_Models;
using RePurpose_Models.Models.Response.Get;
using RePurpose_Models.Models.View;
using RePurpose_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Implementations
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<Guid?> GetWalletById1(Guid id)
        {
            var tran = await _unitOfWork.Wallet.GetMany(product => product.MemberId.Equals(id)).FirstOrDefaultAsync();
            if (tran != null)
            {
                return tran.WalletId;
            }
            return null;
        }

        public async Task<IActionResult> GetWalletById(Guid id)
        {
            var tran = await _unitOfWork.Wallet.GetMany(product => product.MemberId.Equals(id))
                .ProjectTo<WalletViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetAllWalletById()
        {
            var tran = await _unitOfWork.Wallet.GetAll()
                .ProjectTo<WalletViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (tran != null)
            {
                return new JsonResult(tran);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
        public async Task<IActionResult> IPNURL(PaymentInfo vnpay)
        {
            string vnp_HashSecret = "LNMRFLHQQFFKTEZZRQSSMVBYLXFLLFGE";
            long orderId = Convert.ToInt64(vnpay.TxnRef);

            long vnp_Amount = Convert.ToInt64(vnpay.Amount) / 100;
            long vnpayTranId = Convert.ToInt64(vnpay.TransactionNo);
            string vnp_ResponseCode = vnpay.ResponseCode;
            string vnp_TransactionStatus = vnpay.TransactionStatus;
            string vnp_SecureHash = vnpay.SecureHash;
            var transaction = await _unitOfWork.TransactionDb.GetMany(product => product.TransactionId == orderId).FirstOrDefaultAsync();
            var wallet = await _unitOfWork.Wallet.GetMany(product => product.WalletId == transaction.WalletId).FirstOrDefaultAsync();
            if (transaction != null)
            {
                if (transaction.Amount == vnp_Amount)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        if (transaction.Type == "PENDING")
                        {
                            wallet.Balance += transaction.Amount;
                            wallet.LastBalanceUpdate = DateTime.Now;
                            _unitOfWork.Wallet.Update(transaction.Wallet);
                            transaction.Type = "APPROVE";
                            await _unitOfWork.SaveChanges();
                            return new JsonResult(new PaymentInfo
                            {
                                Amount = vnp_Amount,
                                BankCode = vnpay.BankCode,
                                OrderInfo = vnpay.OrderInfo,
                                ResponseCode = vnp_ResponseCode,
                                SecureHash = vnp_SecureHash,
                                TmnCode = vnpay.TmnCode,
                                TransactionNo = vnpayTranId,
                                TransactionStatus = vnp_TransactionStatus,
                                TxnRef = orderId

                            });
                        }

                    }
                    else
                    {
                        transaction.Type = "REJECT";
                        await _unitOfWork.SaveChanges();
                        return new StatusCodeResult(StatusCodes.Status400BadRequest);
                    }
                }
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
    }    
}
