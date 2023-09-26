using AutoMapper;
using RePurpose_Models;
using RePurpose_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Implementations
{
    public class MemberService : BaseService, IMemberService
    {
        public MemberService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<Guid?> GetMemberIdFromToken(ClaimsPrincipal userClaims)
        {
            // Kiểm tra xem userClaims có tồn tại không
            if (userClaims == null)
            {
                return null;
            }

            // Tìm claim có tên là "id"
            var idClaim = userClaims.Claims.FirstOrDefault(c => c.Type == "Id");

            if (idClaim != null)
            {
                // Lấy giá trị của claim "id" và chuyển đổi thành Guid
                if (Guid.TryParse(idClaim.Value, out Guid memberId))
                {
                    return memberId;
                }
            }

            return null; // Trả về null nếu không tìm thấy hoặc không thể chuyển đổi thành Guid
        }

    }
}
