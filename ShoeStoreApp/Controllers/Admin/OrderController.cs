using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreApp.Data;
using ShoeStoreApp.Models;

namespace ShoeStoreApp.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShoeStoreAppContext _context;

        public OrderController(ILogger<HomeController> logger, ShoeStoreAppContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StatusSortParm"] = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewData["NameCustomerSortParm"] = sortOrder == "customerName_asc" ? "customerName_desc" : "customerName_asc";
            ViewData["TotalSortParm"] = sortOrder == "money_asc" ? "money_desc" : "money_asc";
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var orderList = from s in _context.Orders select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                orderList = orderList.Include(c => c.DeliveryAddress).Where(
                    s => s.TimeCreated.ToString().Contains(searchString) ||
                    s.DeliveryAddress.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "status_asc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderBy(e => e.Status);
                    break;
                case "satus_desc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderByDescending(e => e.Status);
                    break;
                case "customerName_asc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderBy(e => e.DeliveryAddress.Name);
                    break;
                case "customerName_desc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderByDescending(e => e.DeliveryAddress.Name);
                    break;
                case "money_asc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderBy(e => e.Total);
                    break;
                case "money_desc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderByDescending(e => e.Total);
                    break;
                case "date_asc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderBy(e => e.TimeCreated);
                    break;
                case "date_desc":
                    orderList = orderList.Include(c => c.DeliveryAddress).OrderByDescending(e => e.TimeCreated);
                    break;

            }
            int pageSize = 10;
            OrderCombine combine = new OrderCombine();
            var paginatedOrder = await PaginatedList<Order>.CreateAsync(orderList, pageNumber ?? 1, pageSize);
            combine.PaginatedListOrder = paginatedOrder;
            //get all order item 
            return PartialView("~/Views/Admin/Order.cshtml", combine);
        }
    }
    public class OrderCombine{
        public PaginatedList<Order> PaginatedListOrder { get;set; }
        
    }
}
