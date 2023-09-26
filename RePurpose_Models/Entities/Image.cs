using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid? ItemImage {  get; set; }
        public string ImageUrl { get; set; }

        public Item Item { get; set; }
    }
}
