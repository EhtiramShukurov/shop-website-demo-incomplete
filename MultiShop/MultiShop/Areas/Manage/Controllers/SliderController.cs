using Microsoft.AspNetCore.Mvc;

namespace MultiShop.Areas.Manage.Controllers
{
    public class SliderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
