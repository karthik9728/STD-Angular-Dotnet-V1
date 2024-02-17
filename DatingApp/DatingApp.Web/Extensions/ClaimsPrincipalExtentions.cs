using System.Security.Claims;

namespace DatingApp.Web.Extensions
{
    public static class ClaimsPrincipalExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
