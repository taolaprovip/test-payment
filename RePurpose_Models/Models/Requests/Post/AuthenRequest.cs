using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.Requests.Post
{
    public class AuthenRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
