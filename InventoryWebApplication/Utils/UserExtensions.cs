using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace InventoryWebApplication.Utils
{
    public static class UserExtensions
    {
        public static string GetClaim(this IEnumerable<Claim> claims, string type)
        {
            return claims.FirstOrDefault(o => o.Type == type)?.Value;
        }
        
        public static string GetRole(this IEnumerable<Claim> claims)
        {
            return claims.GetClaim(ClaimTypes.Role);
        }
        
        public static string GetName(this IEnumerable<Claim> claims)
        {
            return claims.GetClaim(ClaimTypes.Name);
        }
    }
}