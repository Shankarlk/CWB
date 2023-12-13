using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.App.AppUtils
{
    public static class AppUtil
    {
        public static string GetTenantId(ClaimsPrincipal userClaim)
        {
            var user = (userClaim.Identity as ClaimsIdentity);

            return user.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
        }

        public static async Task<Dictionary<string, string>> GetAuthToken(HttpContext httpContext)
        {
            var token = await httpContext.GetTokenAsync("access_token");
            return new Dictionary<string, string> {
                { "Authorization", $"Bearer {token}" }
            };
        }
    }
}
