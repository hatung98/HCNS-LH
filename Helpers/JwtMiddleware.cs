using LACHONG_QLHC_WEB_Contracts.RepoContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_API.Helpers
{
    public class JwtMiddleware
    {
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly RequestDelegate _next;
        public JwtMiddleware(ILogger<JwtMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILoginRepository loginRepo)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, loginRepo, token);

            await _next(context);
        }
        private void attachUserToContext(HttpContext context, ILoginRepository loginRepo, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("lac@hong#tech$13052010");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string userName = jwtToken.Claims.First(x => x.Type == "username").Value;

                context.Items["UserForSession"] = Task.Run(async () => await loginRepo.GetUserLoginByUserName(userName)).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
