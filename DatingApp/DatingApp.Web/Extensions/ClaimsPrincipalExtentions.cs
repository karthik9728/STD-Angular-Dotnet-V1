using System.Security.Claims;

namespace DatingApp.Web.Extensions
{
    public static class ClaimsPrincipalExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId = Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return userId;
        }
    }
}
