using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.View
{
    public class RefreshTokenViewModel
    {
        public Guid Id { get; set; }
        public string TokenValue { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; }
        public Boolean IsActive { get; set; }
        public Guid TokenMember { get; set; }
    }
}
