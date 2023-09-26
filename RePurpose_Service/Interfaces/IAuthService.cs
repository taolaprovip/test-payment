using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Models.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Interfaces
{
    public interface IAuthService
    {
         Task<Member?> Login(AuthenRequest authenRequest);
         Task<AuthenViewModel> GenerateToken(Member member);
    }
}
