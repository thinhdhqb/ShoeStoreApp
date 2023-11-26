using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreApp.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("~/Views/Admin/Order.cshtml");
        }
    }
}
