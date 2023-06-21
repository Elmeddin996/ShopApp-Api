using Microsoft.AspNetCore.Mvc;

namespace Shop.UI.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
