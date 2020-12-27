using DotNetCoreTemplate.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

            #region 在 Main 中取得 DI 註冊的 Service

            // HostBuilder 建置出 Host 的實例之前，要透過 ConfigureServices 先宣告 DI Services
            // HostBuilder 建置出 Host 實例時會依照 ConfigureServices 註冊的配置告知 Service Provider
            // 讓 Service Provider 可以提供請求者 Services
            // 而 Service Provider 的實例會一直存在 Host 的實例中

            // Singleton & Transient 可直接透過 Service Provider 取出
            var singleton = host.Services.GetService<ISampleSingleton>();
            var transient = host.Services.GetService<ISampleTransient>();

            // Scoped 必須先建立 Service Scope，才可以在 Scope 內的 Service Provider 中取出
            using (var scope = host.Services.CreateScope())
            {
                var scoped = scope.ServiceProvider.GetService<ISampleScoped>();
            }

            #endregion

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

                // 在 Generic Host Builder 註冊 DI
                // services.AddScoped<ISampleService, SampleService>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                                             {
                                                 // Web Host 註冊 Services
                                                 Output("[Program] webBuilder.ConfigureServices - Called");


                                                 // 在 Web Host Builder 註冊 DI
                                                 // services.AddScoped<ISampleService, SampleService>();
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
