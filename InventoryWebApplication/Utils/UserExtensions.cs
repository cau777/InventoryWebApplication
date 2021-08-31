using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using InventoryWebApplication.Models;

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

        /// <summary>
        ///     Analyses if the provided ClaimPrincipal has access to a determined role
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimPrincipal of the user to analyser</param>
        /// <param name="desiredRole">The target role</param>
        /// <returns>True if the provided role is equal or more powerful than the target role</returns>
        public static bool HasAccessTo(this ClaimsPrincipal claimsPrincipal, string desiredRole)
        {
            int userRoleIndex = 0;
            int desiredRoleIndex = 0;

            for (int i = 0; i < Role.AvailableRoles.Length; i++)
            {
                if (Role.AvailableRoles[i] == desiredRole) desiredRoleIndex = i;

                if (claimsPrincipal.IsInRole(Role.AvailableRoles[i])) userRoleIndex = i;
            }

            return userRoleIndex <= desiredRoleIndex;
        }
    }
}