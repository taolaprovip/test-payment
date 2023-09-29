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
using VNPAY_CS_ASPX;

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
        /*public async Task<IActionResult> IPNURL(PaymentInfo vnpay)
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = "LNMRFLHQQFFKTEZZRQSSMVBYLXFLLFGE"; ; //Secret key
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //Lay danh sach tham so tra ve tu VNPAY
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    //Cap nhat ket qua GD
                    //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                    //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                    OrderInfo order = new OrderInfo();//get from DB
                    order.OrderId = orderId;
                    order.Amount = 100000;
                    order.PaymentTranId = vnpayTranId;
                    order.Status = "0"; //0: Cho thanh toan,1: da thanh toan,2: GD loi
                    //Kiem tra tinh trang Order
                    if (order != null)
                    {
                        if (order.Amount == vnp_Amount)
                        {
                            if (order.Status == "0")
                            {
                                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                {
                                    //Thanh toan thanh cong
                                    log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId,
                                        vnpayTranId);
                                    order.Status = "1";
                                }
                                else
                                {
                                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                    //  displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                                    log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}",
                                        orderId,
                                        vnpayTranId, vnp_ResponseCode);
                                    order.Status = "2";
                                }

                                //Thêm code Thực hiện cập nhật vào Database 
                                //Update Database

                                returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
                            }
                            else
                            {
                                returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                            }
                        }
                        else
                        {
                            returnContent = "{\"RspCode\":\"04\",\"Message\":\"invalid amount\"}";
                        }
                    }
                    else
                    {
                        returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                    }
                }
                else
                {
                    log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                }
            }
            else
            {
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"Input data required\"}";
            }



            string vnp_HashSecret = "LNMRFLHQQFFKTEZZRQSSMVBYLXFLLFGE";
            long orderId = Convert.ToInt64(vnpay.vnp_TxnRef);

            long vnp_Amount = Convert.ToInt64(vnpay.vnp_Amount) / 100;
            long vnpayTranId = Convert.ToInt64(vnpay.vnp_TransactionNo);
            string vnp_ResponseCode = vnpay.vnp_ResponseCode;
            string vnp_TransactionStatus = vnpay.vnp_TransactionStatus;
            string vnp_SecureHash = vnpay.vnp_SecureHash;
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
                                vnp_Amount = vnp_Amount,
                                vnp_BankCode = vnpay.vnp_BankCode,
                                vnp_OrderInfo = vnpay.vnp_OrderInfo,
                                vnp_ResponseCode = vnp_ResponseCode,
                                vnp_SecureHash = vnp_SecureHash,
                                vnp_TmnCode = vnpay.vnp_TmnCode,
                                vnp_TransactionNo = vnpayTranId,
                                vnp_TransactionStatus = vnp_TransactionStatus,
                                vnp_TxnRef = orderId

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
        }*/
    }    
}
