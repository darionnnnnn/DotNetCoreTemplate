using DotNetCoreTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreTemplate.Filter;
using DotNetCoreTemplate.Middleware;
using DotNetCoreTemplate.Service;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreTemplate.Controllers
{
    // 局部註冊 Middleware
    // [MiddlewareFilter(typeof(FirstMiddleware))]
    // 局部註冊 Filter
    // [TypeFilter(typeof(AuthorizationFilter))] // 繼承 Attribute 後可改寫成 [AuthorizationFilter]
    // 自訂 ActionFilter 執行順序
    // [ActionFilter(Name = "Controller", Order = 2)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISampleService _transientService { get; }
        private ISampleService _scopedService { get; }
        private ISampleService _singletonService { get; }
        public IEnumerable<IPayService> _PayServices { get; }

        public HomeController(ILogger<HomeController> logger,
                              ISampleTransient transientService,
                              ISampleScoped scopedService,
                              ISampleSingleton singletonService,
                              IEnumerable<IPayService> payServices)
        {
            _logger = logger;
            _transientService = transientService;
            _scopedService = scopedService;
            _singletonService = singletonService;

            // One interface with multiple service.
            _PayServices = payServices;
        }

        // 局部註冊 Middleware
        // [MiddlewareFilter(typeof(FirstMiddleware))]
        public IActionResult Index()
        {
            return View();
        }

        // 局部註冊 Filter
        // [TypeFilter(typeof(ActionFilter))] // 繼承 Attribute 後可改寫成 [ActionFilter]
        // 自訂 ActionFilter 執行順序
        // [ActionFilter(Name = "Action", Order = 1)]
        public void PrintFilter()
        {
            Response.WriteAsync("Hello World! \r\n");
        }

        // 局部註冊 Filter
        // [TypeFilter(typeof(ActionFilter))] // 繼承 Attribute 後可改寫成 [ActionFilter]
        public void PrintFilterError()
        {
            throw new System.Exception("Error");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PrintDILifetimeScope()
        {
            ViewBag.Transient = _transientService.GetSomething();
            ViewBag.TransientHashCode = _transientService.GetHashCode();

            ViewBag.Scoped = _scopedService.GetSomething();
            ViewBag.ScopedHashCode = _scopedService.GetHashCode();

            ViewBag.Singleton = _singletonService.GetSomething();
            ViewBag.SingletonHashCode = _singletonService.GetHashCode();

            return View();
        }

        public IActionResult MultipleDI(PayType payType)
        {
            // get needed payService.
            var payService = _PayServices.Single(x => x.PayType == PayType.CashOnDelivery);

            payService.Deduction(100);

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
