using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RePurpose_Models;
using RePurpose_Models.Entities;
using RePurpose_Models.Models.Requests.Post;
using RePurpose_Models.Models.View;
using RePurpose_Models.Repositories.Interfaces;
using RePurpose_Service.Interfaces;
using RePurpose_Utility.Setting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Service.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly AppSetting _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _memberRepository = unitOfWork.Member;
        }
        public async Task<Member?> Login(AuthenRequest authenRequest)
        {
            var member = await _unitOfWork.Member.GetMany(member => member.Email.Equals(authenRequest.Email)
            && member.PasswordHash.Equals(authenRequest.Password)).FirstOrDefaultAsync();
            if (member == null)
            {
                return null;
            }
            return member;
        }

        public async Task<AuthenViewModel> GenerateToken(Member member)
        {
            if(member == null)
            {
                return null;
            }
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, member.Name),
                    new Claim(JwtRegisteredClaimNames.Email, member.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, member.Email),                              
                    new Claim("Id", member.Id.ToString()),

                    //roles
                }),
                Expires = DateTime.UtcNow.AddHours(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Lưu database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                TokenValue = refreshToken,
                IssuedAt = DateTime.UtcNow,
                IsActive = false,
                TokenMember = member.Id,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            await _unitOfWork.RefreshToken.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChanges();

            return new AuthenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }
    }
}
