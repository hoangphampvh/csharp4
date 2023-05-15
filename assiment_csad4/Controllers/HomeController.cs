using assiment_csad4.Configruration;
using assiment_csad4.IService;
using assiment_csad4.Models;
using assiment_csad4.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace assiment_csad4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProduct _serviceProduct;
        public HomeController(ILogger<HomeController> logger, ProductService productService)
        {
            _serviceProduct = productService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(_serviceProduct.GetAllProduct().OrderByDescending(p => p.CreateDate).Take(6));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}