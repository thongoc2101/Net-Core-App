using System.Linq;
using System.Security.Claims;

namespace NetCoreApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type.Equals(claimType));
            return claim != null ? claim.Value : string.Empty;
        }
    }
}
