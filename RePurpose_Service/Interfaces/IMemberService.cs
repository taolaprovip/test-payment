using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Interfaces
{
    public interface IMemberService
    {
        Task<Guid?> GetMemberIdFromToken(ClaimsPrincipal userClaims);
    }
}
