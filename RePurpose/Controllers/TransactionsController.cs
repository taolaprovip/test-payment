using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Service.Interfaces;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMemberService _memberService;
        private readonly IWalletService _walletService;

        public TransactionsController(ITransactionService transactionService, IMemberService memberService, IWalletService walletService)
        {
            _transactionService = transactionService;
            _memberService = memberService;
            _walletService = walletService;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAllTransaction()
        {
            try
            {
              
                return await _transactionService.GetAllTransaction();
            }
            catch
            {
                return BadRequest("Don't");
            }

           
        }
        [HttpGet("get-my-transaction")]
        [Authorize]
        public async Task<IActionResult> GetTransaction()
        {
            var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);

            var rs = await _walletService.GetWalletById1(idClaim.Value); ;
            if (rs != null)
            {
                return await _transactionService.getmytransaction(rs);
            }
            return BadRequest("Not Bad");
        }
        // POST: api/Transactions
        [HttpPost("create-transaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateModel transactionCreateModel)
        {
            try
            {
                var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);
                var rs = await _transactionService.CreateTransaction(transactionCreateModel, idClaim.Value);
                if (rs is JsonResult jsonResult)
                {
                    if (jsonResult.Value is null) return BadRequest("Message: Not Bad");
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                return StatusCode(StatusCodes.Status400BadRequest, "Bad");

            }
            catch
            {
                return BadRequest("Don't Create");
            }
        }
    }
}
