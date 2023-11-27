using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreApp.Models;

namespace ShoeStoreApp.Controllers.Admin
{
    //[Route("admin/[controller]")]
   // public class OrderController : Controller
   // {
   //     public async Task<IActionResult> Index(string sortOrder,
   //         string currentFilter,
   //         string searchString,
   //         int? pageNumber)
   //     {
   //         ViewData["CurrentSort"] = sortOrder;
   //         ViewData["StatusSortParm"] = sortOrder == "status_asc" ? "status_desc" : "status_asc";
   //         ViewData["NameCustomerSortParm"] = sortOrder == "customerName_asc" ? "customerName_desc" : "customerName_asc";
			//ViewData["AddressSortParm"] = sortOrder == "address_asc" ? "address_desc" : "address_asc";
			//ViewData["MoneySortParm"] = sortOrder == "money_asc" ? "money_desc" : "money_asc";
			//ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
			//if (searchString != null)
   //         {
   //             pageNumber = 1;
   //         }
   //         else
   //         {
   //             searchString = currentFilter;
   //         }
   //         ViewData["CurrentFilter"] = searchString;
   //         var listOrder = .Users;
   //         if (!String.IsNullOrEmpty(searchString))
   //         {
   //             listUser = listUser.Where(s => s.Email.Contains(searchString));
   //         }
   //         switch (sortOrder)
   //         {
   //             case "email_desc":
   //                 listUser = listUser.OrderByDescending(s => s.Email); break;
   //             case "email_asc":
   //                 listUser = listUser.OrderBy(s => s.Email); break;
   //             case "status_asc":
   //                 listUser = listUser.OrderBy(s => s.EmailConfirmed); break;
   //             case "status_desc":
   //                 listUser = listUser.OrderByDescending(s => s.EmailConfirmed); break;
   //         }
   //         int pageSize = 10;
   //         CombineCustomer combine = new CombineCustomer();
   //         var paginatedCustomer = await PaginatedList<ApplicationUser>.CreateAsync(listUser, pageNumber ?? 1, pageSize);
   //         combine.PaginatedListCustomer = paginatedCustomer;
   //         return PartialView("~/Views/Admin/Customer.cshtml", combine);
   //     }
   // }
}
