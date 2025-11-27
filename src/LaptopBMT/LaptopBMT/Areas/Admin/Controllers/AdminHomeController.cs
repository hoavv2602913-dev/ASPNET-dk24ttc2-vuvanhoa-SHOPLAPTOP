using Microsoft.AspNetCore.Mvc;

namespace LaptopBMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Microsoft.AspNetCore.Mvc.Controller

    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
