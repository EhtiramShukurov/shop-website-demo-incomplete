using Microsoft.AspNetCore.Mvc;

namespace MultiShop.Areas.Manage.Controllers
{
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
