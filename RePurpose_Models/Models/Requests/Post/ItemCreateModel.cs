using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RePurpose_Models.Entities;

namespace RePurpose_Models.Models.Requests.Post
{
    public class ItemCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string PickupTime { get; set; }
        public LocationCreateModel Location { get; set; }
        public Guid CategoryId { get; set; }
        public List<IFormFile> files { get; set; } = new List<IFormFile>();
    }
}
