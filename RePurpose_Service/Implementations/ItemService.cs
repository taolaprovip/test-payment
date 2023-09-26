using AutoMapper;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RePurpose_Models;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Implementations
{
    public class ItemService : BaseService, IItemService
    {
        public ItemService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IActionResult> PostItem(ItemCreateModel itemCreateModel, Guid id)
        {
            if (itemCreateModel.files == null || itemCreateModel.files.Count > 10 || itemCreateModel.files.Count == 0)
            {
                return new StatusCodeResult(300);
            }

            var member = await _unitOfWork.Member.FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return new StatusCodeResult(400);
            }

            if (itemCreateModel.CategoryId == Guid.Empty)
            {
                return new StatusCodeResult(401);
            }

            var locationMemberId = await _unitOfWork.Location.FirstOrDefaultAsync(location => location.LocationMember == member.Id);

            Item item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemCreateModel.Name,
                Description = itemCreateModel.Description,
                Type = "Give",
                Quantity = itemCreateModel.Quantity,
                IsDeleted = false,
                PickupTime = itemCreateModel.PickupTime,
                CategoryId = itemCreateModel.CategoryId,
                GiverId = member.Id
            };

            if (locationMemberId != null)
            {
                item.ItemLocation = locationMemberId.Id;
            }
            else
            {
                Location location = new Location
                {
                    Id = Guid.NewGuid(),
                    Latitude = itemCreateModel.Location.Latitude,
                    Longitude = itemCreateModel.Location.Longitude
                };
                await _unitOfWork.Location.AddAsync(location);
                item.ItemLocation = location.Id;
            }

            await _unitOfWork.Item.AddAsync(item);

            foreach (var file in itemCreateModel.files)
            {
                var imageItem = new Image
                {
                    Id = Guid.NewGuid(),
                    ItemImage = item.Id,
                    ImageUrl = await UploadProductImageToFirebase(file)
                };
                await _unitOfWork.Image.AddAsync(imageItem);
            }

            await _unitOfWork.SaveChanges();
            return new StatusCodeResult(200);
        }


        private async Task<string?> UploadProductImageToFirebase(IFormFile file)
        {
            var storage = new FirebaseStorage("push-notification-5147f.appspot.com");
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child("images")
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }
}
}
