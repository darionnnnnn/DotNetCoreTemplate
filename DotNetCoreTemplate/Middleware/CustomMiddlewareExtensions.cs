using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace DotNetCoreTemplate.Middleware
{
    // 以靜態方法包裝 Middleware，通常用於擴充(Extensions) (e.g. UseRouting(), UseRewriter())
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirstMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FirstMiddleware>();
        }
    }
}
