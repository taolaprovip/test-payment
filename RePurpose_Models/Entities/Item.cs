using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }// xác nhận
        public int Quantity { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }// ngày nhận
        public string PickupTime { get; set; }
        public Guid? GiverId { get; set; } // Khóa ngoại cho người cho
        public Guid? ReceiverId { get; set; } // Khóa ngoại cho người nhận}
        public Guid? ItemLocation {  get; set; }
        public Guid? CategoryId { get; set; }

        public Member Giver { get; set; }
        public Member Receiver { get; set; }
        public Category Category { get; set; }
        public ICollection<Image> Images { get; set; }
        public Location Location { get; set; }

    }
}
