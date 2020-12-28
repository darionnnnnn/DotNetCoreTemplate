﻿using DotNetCoreTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreTemplate.Middleware;
using DotNetCoreTemplate.Service;

namespace DotNetCoreTemplate.Controllers
{
    // 局部註冊 Middleware
    // [MiddlewareFilter(typeof(FirstMiddleware))]
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
