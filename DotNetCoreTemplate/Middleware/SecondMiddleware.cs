using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreTemplate.Middleware
{
    public class SecondMiddleware
    {
        private readonly RequestDelegate _next;

        public SecondMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 讀取 Response.Body
            // Response.Body 的 Stream 不允許讀取，但可替換
            // Response.Body 開始寫入前指向 MemoryStream，之後 Pipeline 寫入 Response.Body 時，實際上是寫入到 MemoryStream 中
            // 等下層 Pipeline 做完時，再把 MemoryStream 寫回 Response.Body

            string responseContent;
            var originalBodyStream = context.Response.Body;
            
            using (var fakeResponseBody = new MemoryStream())
            {
                context.Response.Body = fakeResponseBody;

                await _next(context);

                fakeResponseBody.Seek(0, SeekOrigin.Begin);
                
                using (var reader = new StreamReader(fakeResponseBody))
                {
                    responseContent = await reader.ReadToEndAsync();
                    fakeResponseBody.Seek(0, SeekOrigin.Begin);

                    await fakeResponseBody.CopyToAsync(originalBodyStream);
                }
            }
            
            // Console.WriteLine($"Response.Body={responseContent}");
        }
    }
}
