using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCoreTemplate.Middleware;
using DotNetCoreTemplate.Service;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Program.Output("[Startup] Constructor - Called");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Host 建置時 WebHost 會呼叫 UseStartup 泛型類別的 ConfigureServices 方法
        // This method gets called by the runtime. Use this method to add services to the container.
        // 在此將服務註冊到 DI 容器用，可不實做
        public void ConfigureServices(IServiceCollection services)
        {
            Program.Output("[Startup] WebHost ConfigureServices - Called");
            services.AddControllersWithViews();

            #region DI (Dependency Injection) 

            // services 就是一個 DI 容器，把 MVC 的服務註冊到 DI 容器，需要用 MVC 服務時，才從 DI 容器取得物件實例
            services.AddMvc();
            // 註冊 interface & 實作，也可在 Program 中註冊，但不可重複註冊
            services.AddTransient<ISampleTransient, SampleService>();
            services.AddScoped<ISampleScoped, SampleService>();
            services.AddSingleton<ISampleSingleton, SampleService>();

            #region 同一 interface 註冊多個 service

            // 建議只在 Singleton 使用，Transient & Scoped 注入時會 new 新的實例，沒用到就變成不必要的效能耗損
            services.AddSingleton<IPayService, CashOnDeliveryService>();
            services.AddSingleton<IPayService, CreditCardService>();
            services.AddSingleton<IPayService, WhateverPayService>();

            #endregion

            #region 在 Host 實例產生前取得 Service Provider

            // 在 Host 實例產生後即可透過 Constructor 注入取得 Service
            // 在 Host 實例產生前，則可透過 IServiceCollection 取得
            // // 建置 Service Provider (與 Host 中的 Service Provider 是不同實體，各自擁有各自的 Singleton)
            // var serviceProvider = services.BuildServiceProvider();
            //
            // // 從 Service Provider 取得服務使用
            // var sample = serviceProvider.GetService<ISampleSingleton>();

            #endregion

            // Singleton 也可以用以下方法註冊
            // services.AddSingleton<ISampleSingleton>(new SampleService());

            // 也可以透過委派的方式註冊
            // services.AddTransient<ISampleTransient>(srv => {
            //                                             var sampleService = new SampleService();
            //                                             // Do something ...
            //                                             return sampleService;
            //                                         });
            // services.AddScoped<ISampleScoped>(srv => new SampleService());
            // services.AddSingleton<ISampleSingleton>(srv => new SampleService());
            #endregion

        }

        // Host 啟動後 WebHost 會呼叫 UseStartup 泛型類別的 Configure 方法
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            #region ApplicationLifetime
            appLifetime.ApplicationStarted
                       .Register(() =>
                                 {
                                     Program.Output("[Startup] ApplicationLifetime - Started");
                                 });

            appLifetime.ApplicationStopping
                       .Register(() =>
                                 {
                                     Program.Output("[Startup] ApplicationLifetime - Stopping");
                                 });

            appLifetime.ApplicationStopped
                       .Register(() =>
                                 {
                                     Thread.Sleep(5 * 1000);
                                     Program.Output("[Startup] ApplicationLifetime - Stopped");
                                 });
            #endregion

            #region Middleware
            // 全域註冊外部 Middleware，亦可在 Controlle 上加 Attribute 做局部註冊
            // 註冊 FirstMiddleware
            // app.UseMiddleware<FirstMiddleware>();

            // 註冊 SecondMiddleware
            // app.UseMiddleware<SecondMiddleware>();

            // 註冊靜態的擴充 Middleware
            // app.UseFirstMiddleware();

            // 實務上會將 Middleware 分開放以便維護
            // app.Use(async (context, next) =>
            //         {
            //             await context.Response.WriteAsync("First Middleware in. \r\n");
            //             await next.Invoke();
            //             await context.Response.WriteAsync("First Middleware out. \r\n");
            //         });
            //
            // // 作為路由，依據 URL (/second) 決定是否
            // app.Map("/second", mapApp =>
            //                    {
            //                        mapApp.Use(async (context, next) =>
            //                                   {
            //                                       await context.Response.WriteAsync("Second Middleware in. \r\n");
            //                                       await next.Invoke();
            //                                       await context.Response.WriteAsync("Second Middleware out. \r\n");
            //                                   });
            //                        mapApp.Run(async context =>
            //                                   {
            //                                       await context.Response.WriteAsync("Second. \r\n");
            //                                   });
            //                    });
            // app.Use(async (context, next) =>
            //         {
            //             await context.Response.WriteAsync("Third Middleware in. \r\n");
            //             await next.Invoke();
            //             await context.Response.WriteAsync("Third Middleware out. \r\n");
            //         });
            //

            // Run 是 Middleware 的最後一個行為
            // app.Run(async context =>
            //         {
            //             await context.Response.WriteAsync("Hello World! \r\n");
            //         });
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });



            // For trigger stop WebHost
            // var thread = new Thread(() =>
            //                         {
            //                             Thread.Sleep(5 * 1000);
            //                             Program.Output("[Startup] Trigger stop WebHost");
            //                             appLifetime.StopApplication();
            //                         });
            // thread.Start();

            Program.Output("[Startup] Configure - Called");
        }
    }
}
