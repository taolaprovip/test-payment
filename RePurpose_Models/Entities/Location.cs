using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid? LocationMember {  get; set; }
        public Member Member {  get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
