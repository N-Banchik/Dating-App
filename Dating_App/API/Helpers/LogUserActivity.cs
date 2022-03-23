using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Entities;
using API.Extensions;
using API.interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext resultcontext= await next();
            if(!resultcontext.HttpContext.User.Identity.IsAuthenticated) return;
            int userId = resultcontext.HttpContext.User.GetUserId();
            IUserRepository repository = resultcontext.HttpContext.RequestServices.GetService<IUserRepository>();
            AppUser user = await repository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await repository.SaveAllAsync();
        }
    }
}