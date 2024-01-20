using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Z.Fantasy.Core.Attributes;
using Z.Fantasy.Core.Helper;
using Z.SunBlog.Application.UserModule;
using Z.SunBlog.Host.Models;

namespace Z.SunBlog.Host.Controllers
{
    [NoResult]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserAppService userRespo;

        public HomeController(ILogger<HomeController> logger,  IUserAppService userRespo)
        {
            _logger = logger;
            this.userRespo = userRespo;
        }
        public async Task<IActionResult> Index()
        {
            if (AppSettings.Environment().IsDevelopment())
            {
                ViewBag.Url = "http://localhost:5155";
            }
            else
            {
                ViewBag.Url = "http://8.134.249.130:5155";
            }
            
            _logger.LogInformation("正在加载首页......");
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}