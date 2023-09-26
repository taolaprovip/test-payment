using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RePurpose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMemberService _memberService;

        public ItemsController(IItemService itemService, IMemberService memberService)
        {
            _itemService = itemService;
            _memberService = memberService;
        }

        [HttpPost("post-item")]
        public async Task<IActionResult> PostItem([FromForm] ItemCreateModel itemCreateModel)
        {
            try
            {   
                var idClaim = await _memberService.GetMemberIdFromToken(HttpContext.User);
                var rs =  await _itemService.PostItem(itemCreateModel, idClaim.Value);
                if (rs is StatusCodeResult status)
                {
                    if (status.StatusCode == 300) return StatusCode(StatusCodes.Status400BadRequest, "Please input image");
                    if (status.StatusCode == 400) return StatusCode(StatusCodes.Status400BadRequest);
                    if (status.StatusCode == 401) return StatusCode(StatusCodes.Status400BadRequest, "Please input category");
                    if (status.StatusCode == 200) return StatusCode(StatusCodes.Status201Created);
                }
                return BadRequest("Don't post item");
                
            }
            catch 
            {
                return BadRequest("Don't post item");
            }
        }

       /* [HttpPost]
        public async Task<IActionResult> img([FromForm] IFormFile file)
        {

            try
            {
                var imga = await UploadProductImageToFirebase(file);

                
                return Ok(imga);

            }
            catch (Exception ex)
            {
                return BadRequest("Can't set location");
            }
        }
        private async Task<string?> UploadProductImageToFirebase(IFormFile file)
        {
            var storage = new FirebaseStorage("push-notification-5147f.appspot.com");
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child("images")
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }*/
    }
}
