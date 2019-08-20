using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace FrontEnd.Filter
{
    public class RequireLoginFilter : IAsyncResourceFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public RequireLoginFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(context);

            if (context.HttpContext.User.Identity.IsAuthenticated &&
                !context.Filters.OfType<SkipWelcomeAttribute>().Any())
            {
                var isAttendee = context.HttpContext.User.IsAttendee();

                if (!isAttendee)
                {
                    context.HttpContext.Response.Redirect(urlHelper.Page("/Welcome"));

                    return;
                }
            }

            await next();
        }
    }
}
