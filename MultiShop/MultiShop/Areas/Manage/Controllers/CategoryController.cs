using Microsoft.AspNetCore.Mvc;

namespace MultiShop.Areas.Manage.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
