using System;
using System.Threading.Tasks;
using MAV.Chat.Common.Extensions;
using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
namespace MAV.Chat.Common.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var resultContext = await next();

           if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

           var userId = resultContext.HttpContext.User.GetUserId();
           var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
           var user = await uow.Repository<MavUser>().GetByIdAsync(userId);
           user.LastActive = DateTime.UtcNow;
           await uow.SaveChangesAsync();
        }
    }
}