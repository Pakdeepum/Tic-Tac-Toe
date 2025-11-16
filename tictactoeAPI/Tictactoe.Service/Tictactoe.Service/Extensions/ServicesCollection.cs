using Microsoft.IdentityModel.Tokens;
using Tictactoe.Service.DAL;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Tictactoe.Service.DAL.Interface;
using Tictactoe.Service.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Tictactoe.Service.Extensions
{
    public static class ServicesCollection
    {
        
        public static IServiceCollection InjectServicesCollection(this IServiceCollection services)
        {
            services.AddScoped<IUserDAL, UserDAL>();
            services.AddScoped<IUserScoreLogDAL, UserScoreLogDAL>();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

            return services;
        }
    }
}
