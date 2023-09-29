using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using RePurpose_Models;
using RePurpose_Models.Repositories.Implementations;
using RePurpose_Models.Repositories.Interfaces;
using RePurpose_Service.Implementations;
using RePurpose_Service.Interfaces;
using RePurpose_Utility.Setting;
using System.Text;
using VNPAY_CS_ASPX;

namespace RePurpose.Configurations
{
    public static class AppConfiguration
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            
            var secretKey = configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tự cấp token
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ký vào token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddHttpContextAccessor();


            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
