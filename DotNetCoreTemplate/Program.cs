using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DotNetCoreTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Output("[Program] Start");
            Output("[Program] Create HostBuilder");
            var hostBuilder = CreateHostBuilder(args);

            Output("[Program] Build Host");
            // 實例化 Host & WebHost
            var host = hostBuilder.Build();

            Output("[Program] Run Host");
            // 啟動 Host
            host.Run();

            Output("[Program] End");
        }

        /// <summary>
        /// 宣告相依的服務及組態設定包含 WebHost
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            // 可不使用 Startup 直接註冊 ConfigureServices & Configure
            .ConfigureServices(services =>
            {
                // Generic Host 註冊 Services
                Output("[Program] hostBuilder.ConfigureServices - Called");
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                                             {
                                                 // Web Host 註冊 Services
                                                 Output("[Program] webBuilder.ConfigureServices - Called");
                                             })
                          .Configure(app =>
                                     {
                                         // Configure 是唯一的，註冊會後蓋前，所以此行不會顯示
                                         Output("[Program] webBuilder.Configure - Called");

                                         // app.UseRouting();
                                         // app.UseEndpoints(endpoints =>
                                         //                  {
                                         //                      endpoints.MapGet("/",
                                         //                                       async context => {
                                         //                                           await context.Response.WriteAsync("Hello World!");
                                         //                                       });
                                         //                  });
                                     })
                          .UseStartup<Startup>();
            });
        //     .ConfigureWebHostDefaults(webBuilder =>
        //     {
        //         // 設定 WebHostBuilder 產生的 WebHost 時執行的類別
        //         webBuilder.UseStartup<Startup>();
        // });

        public static void Output(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] {message}");
        }
    }
}
