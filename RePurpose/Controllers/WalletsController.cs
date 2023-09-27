using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Service.Interfaces;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IMemberService _memberService;

        public WalletsController(IWalletService walletService, IMemberService memberService)
        {
            _walletService = walletService;
            _memberService = memberService;
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
    }
}
