using Microsoft.AspNetCore.Mvc;

namespace ShoeStoreApp.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("~/Views/Admin/Customer.cshtml");
        }
    }
}
