using Tictactoe.Service.Models.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tictactoe.Service
{
    public class AuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] operationCodes;
        private readonly int[] roles;

        public AuthorizedAttribute(params string[] operationCodes)
        {
            this.operationCodes = operationCodes;
            this.roles = Array.Empty<int>();
        }

        public AuthorizedAttribute(params int[] roles)
        {
            this.operationCodes = Array.Empty<string>();
            this.roles = roles;
        }

        public AuthorizedAttribute()
        {
            this.operationCodes = Array.Empty<string>();
            this.roles = Array.Empty<int>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Resolve IConfiguration จาก HttpContext
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            if (configuration == null)
            {
                context.Result = new ObjectResult(new APIResponse(false, "Server error: Missing configuration", 500))
                {
                    StatusCode = 500
                };
                return;
            }

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1] ?? string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ObjectResult(new APIResponse(false, "Unauthorized", 401))
                {
                    StatusCode = 401
                };
                return;
            }

            var result = ValidateJwt(token, configuration);

            if (result.IsValid)
            {
                if (result.IsExpired)
                {
                    context.Result = new ObjectResult(new APIResponse(false, "Expired", 401))
                    {
                        StatusCode = 401
                    };
                    return;
                }
            }
            else
            {
                context.Result = new ObjectResult(new APIResponse(false, "Unauthorized", 401))
                {
                    StatusCode = 401
                };
                return;
            }
        }

        private (bool IsValid, bool IsExpired, ClaimsPrincipal Claims) ValidateJwt(string token, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            string set = configuration["JWT:Set"] ?? string.Empty;
            var key = Encoding.ASCII.GetBytes(set);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtToken) ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm.");
                }

                var isExpired = jwtToken.ValidTo < DateTime.UtcNow;

                return (true, isExpired, claimsPrincipal);
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, true, null);
            }
            catch (Exception)
            {
                return (false, false, null);
            }
        }
    }
}