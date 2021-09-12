using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MAV.Chat.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MAV.Chat.Common.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<MavUser> FindByEmailFromClaimsPrincipal(this UserManager<MavUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}