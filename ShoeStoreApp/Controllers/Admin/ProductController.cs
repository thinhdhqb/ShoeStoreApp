using Microsoft.AspNetCore.Mvc;
using ShoeStoreApp.Data;
using ShoeStoreApp.Models;

namespace ShoeStoreApp.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShoeStoreDbContext _context;

        public ProductController(ILogger<HomeController> logger, ShoeStoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return PartialView("~/Views/Admin/Product.cshtml");
        }
        [HttpPost]
        [Route("/AddProduct", Name ="AddProduct")]
        public IActionResult AddProduct([Bind("Name","Price", "Description","Brand","Gender", "Category")] Product product)
        {
            _context.Products.Add(product);


            return RedirectToAction("Index");
        }
    }
}
