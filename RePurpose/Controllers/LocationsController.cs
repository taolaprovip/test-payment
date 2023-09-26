using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly ILocationService _locationService;

        public LocationsController(IMemberService memberService, ILocationService locationService)
        {
            _memberService = memberService;
            _locationService = locationService;
        }

        [HttpPost("setLocation")]
        [Authorize]
        public async Task<IActionResult> SetLocation([FromBody][Required] LocationCreateModel location)
        {
           
            try
            {
                var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);
                var setLocation = await _locationService.SetLocation(location, idClaim.Value);
              
                if (setLocation is StatusCodeResult status)
                {
                    if (status.StatusCode == 400) return StatusCode(StatusCodes.Status400BadRequest);
                    if (status.StatusCode == 200) return StatusCode(StatusCodes.Status201Created);
                }
                return BadRequest("Can't set location");

            }
            catch (Exception ex)
            {
                return BadRequest("Can't set location");
            }
        }
        [HttpGet("getUserId")]
        [Authorize] // Yêu cầu xác thực bằng token
        public async Task<Guid?> GetUserId()
        {
            

            // Tìm claim có tên là "id"
            var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);

            if (idClaim != null)
            {
                
                return idClaim;
            }

            return null;
           
        }
    }
}
