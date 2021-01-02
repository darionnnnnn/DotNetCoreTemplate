using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCoreTemplate.Filter
{
    // 同步
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
        }
    }

    // 非同步
    // public class ExceptionFilter : IAsyncExceptionFilter
    // {
    //     public Task OnExceptionAsync(ExceptionContext context)
    //     {
    //         context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
    //         return Task.CompletedTask;
    //     }
    // }
}
