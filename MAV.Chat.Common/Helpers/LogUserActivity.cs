using System;
using System.Threading.Tasks;
using MAV.Chat.Common.Extensions;
using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
namespace MAV.Chat.Common.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userName = resultContext.HttpContext.User.GetUserName();
            var userManager = resultContext.HttpContext.RequestServices.GetService<UserManager<MavUser>>();
            var user = await userManager.FindByNameAsync(userName);
            user.LastActive = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }
    }
}