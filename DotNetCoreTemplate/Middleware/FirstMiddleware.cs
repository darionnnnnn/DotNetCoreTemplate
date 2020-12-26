using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreTemplate.Middleware
{
    public class FirstMiddleware
    {
        private readonly RequestDelegate _next;

        public FirstMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // HttpContext 包含 URL、Header、Body，其中 Body 是 stream 類別
        public async Task Invoke(HttpContext context)
        {
            // await context.Response.WriteAsync($"{nameof(FirstMiddleware)} in. \r\n");
            // await _next(context);
            // await context.Response.WriteAsync($"{nameof(FirstMiddleware)} out. \r\n");

            // 讀取 Request.Body
            string requestContent;

            using (var reader = new StreamReader(context.Request.Body))
            {
                requestContent = await reader.ReadToEndAsync();
                // 還原讀取位置
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            await _next(context);

            // Console.WriteLine($"Request.Body={requestContent}");
        }
    }
}
