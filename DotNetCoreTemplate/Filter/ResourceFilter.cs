using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCoreTemplate.Filter
{
    // 同步
    public class ResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }


    }

    // 非同步
    // public class ResourceFilter : IAsyncResourceFilter
    // {
    //     public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    //     {
    //         await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
    //
    //         await next();
    //
    //         await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
    //     }
    // }
}
