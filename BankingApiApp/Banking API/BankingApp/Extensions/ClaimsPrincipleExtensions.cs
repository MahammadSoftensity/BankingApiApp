using System.Security.Claims;

namespace BankingApp.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static int GetUserId(this ClaimsPrincipal userClaimsPrincipal)
        {
            return int.Parse(userClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
