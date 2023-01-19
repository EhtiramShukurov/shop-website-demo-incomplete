using Microsoft.AspNetCore.Mvc;

namespace MultiShop.Areas.Manage.Controllers
{
    public class OfferController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
