using Microsoft.AspNetCore.Mvc;

namespace MultiShop.Areas.Manage.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
