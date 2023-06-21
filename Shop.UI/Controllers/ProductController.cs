using Microsoft.AspNetCore.Mvc;

namespace Shop.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
