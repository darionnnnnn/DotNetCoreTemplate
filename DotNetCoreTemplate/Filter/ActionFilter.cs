using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCoreTemplate.Filter
{
    // 同步
    // Attribute : 簡化 Controller & Action 上 Attribute 寫喇
    // IOrderedFilter : 自訂全域 & 局部 (Controller & Action) 的 ActionFilter 執行順序
    public class ActionFilter : Attribute, IActionFilter, IOrderedFilter
    {
        public string Name { get; set; }
        public int Order { get; set; } = 0;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }
    }

    // 非同步
    // public class ActionFilter : IAsyncActionFilter
    // {
    //     public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //     {
    //         await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
    //
    //         await next();
    //
    //         await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
    //     }
    // }
}
