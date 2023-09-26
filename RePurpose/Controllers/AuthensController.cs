using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthensController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthensController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenMember([FromBody][Required] AuthenRequest authen)
        {
            try
            {
                var member = await _authService.Login(authen);
                if (member != null)
                {
                    var login = await _authService.GenerateToken(member);
                    return Ok(login);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
            
        }
    }
}
