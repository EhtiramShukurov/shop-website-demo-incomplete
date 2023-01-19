using Microsoft.AspNetCore.Mvc;

namespace MultiShop.Areas.Manage.Controllers
{
    public class DiscountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
