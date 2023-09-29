using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Service.Interfaces;
using VNPAY_CS_ASPX;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;
        private readonly IWalletService _walletService;


        public WalletsController(ITransactionService transactionService, IMemberService memberService, IWalletService walletService)
        {
            _transactionService = transactionService;
            _memberService = memberService;
            _walletService = walletService;
        }

        [HttpGet("getAllWallet")]

        public async Task<IActionResult> GetAllWallet()
        {


            return await _walletService.GetAllWalletById();
        }
        [HttpGet("get-my-wallet")]
        [Authorize]
       
        public async Task<IActionResult> GetWallettome()
        {
            try
            {
                var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);
                return await  _walletService.GetWalletById(idClaim.Value);
            } catch
            {
                return BadRequest("Don't Get");
            }
           
        }

        [HttpGet("/process-payment")]
        public async Task<IActionResult> ProcessPayment()
        {
            string returnContent = string.Empty;

            try
            {
                string vnp_HashSecret = "LNMRFLHQQFFKTEZZRQSSMVBYLXFLLFGE";
                var vnpayData = new Dictionary<string, string>();

                foreach (var key in Request.Query.Keys)
                {
                    var values = Request.Query[key];
                    if (values.Count > 0)
                    {
                        vnpayData[key] = values[0];
                    }
                }

                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (var entry in vnpayData)
                {
                    string key = entry.Key;
                    string value = entry.Value;

                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value);
                    }
                }


                // Lấy thông tin từ Query String
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = Request.Query["vnp_SecureHash"].ToString(); // Lấy giá trị của tham số vnp_SecureHash và chuyển đổi thành chuỗi

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

                var trans = await  _transactionService.GetTransactionById(orderId);

                if (checkSignature)
                {
                    

                    if (trans != null)
                    {
                        if (trans.Amount == vnp_Amount)
                        {
                            if (trans.Type == "PENDING")
                            {
                                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                {
                                    // Thanh toán thành công
                                    trans.Type = "APPROVE";
                                    returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
                                }
                                else
                                {
                                    // Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                                    trans.Type = "REJECT";
                                    returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                                }

                                // Cập nhật thông tin đơn hàng vào CSDL
                                _transactionService.UpdateOrderInfoInDatabase(trans);
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
                    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"An error occurred\"}";
            }

            return Content(returnContent, "application/json");
        }

       
    }
}
