using System.Security.Claims;

namespace MAV.Chat.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}