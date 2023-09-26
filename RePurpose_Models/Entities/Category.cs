using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Boolean IsDeleted { get; set; }
        public int PointX { get; set; }
        public int PointY { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}
