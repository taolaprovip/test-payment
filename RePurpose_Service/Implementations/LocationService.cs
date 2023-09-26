using AutoMapper;
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
    public class LocationService : BaseService, ILocationService
    {
        public LocationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<IActionResult> SetLocation(LocationCreateModel locationCreateModel, Guid memberId)
        {
            if (locationCreateModel.Longitude == null || locationCreateModel.Latitude == null)
            {
                return new StatusCodeResult(400); ;
            }

            Location location = new Location
            {
                Id = Guid.NewGuid(),
                Latitude = locationCreateModel.Latitude,
                Longitude = locationCreateModel.Longitude,
                LocationMember = memberId

            };
                await _unitOfWork.Location.AddAsync(location);
                await _unitOfWork.SaveChanges();       
            return new StatusCodeResult(200);
        }
    }
}
