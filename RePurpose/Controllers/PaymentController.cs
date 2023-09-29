using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Service.Interfaces;
using VNPAY_CS_ASPX;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;
        private readonly IWalletService _walletService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(ITransactionService transactionService, IMemberService memberService, IWalletService walletService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _memberService = memberService;
            _walletService = walletService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("link-payment")]
        [Authorize]
        public async Task<IActionResult> ProcessPayment()
        {
            try
            {
                var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);
                var rs = await _walletService.GetWalletById1(idClaim.Value);
                var trans = await _transactionService.getmytransaction1(rs);

                string vnp_Returnurl = "https://www.facebook.com/tranthe2uang/"; //URL nhan ket qua tra ve 
                string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = "0EDCIZXP"; //Ma định danh merchant kết nối (Terminal Id)
                string vnp_HashSecret = "LNMRFLHQQFFKTEZZRQSSMVBYLXFLLFGE"; //Secret Key

                VnPayLibrary vnpay = new VnPayLibrary();


                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", Math.Floor(decimal.Parse(trans.Amount.ToString()) * 100).ToString());

                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor));

                vnpay.AddRequestData("vnp_Locale", "vn");


                vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + trans.TransactionId);
                vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", trans.TransactionId.ToString());



                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
      
    }
}
