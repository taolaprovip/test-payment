using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Boolean PhoneNumberConfirmed { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public Boolean IsDeleted {  get; set; }
        public string Image {  get; set; }

        public DateTime CreateAt { get; set; }
        public Location Location { get; set; }
        public Wallet Wallet { get; set; }
        public ICollection<Item> ItemsGiven { get; set; }
        public ICollection<Item> ItemsReceived { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }



    }
}
