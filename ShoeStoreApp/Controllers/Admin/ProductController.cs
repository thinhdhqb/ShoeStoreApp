using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoeStoreApp.Data;
using ShoeStoreApp.Models;
using ShoeStoreApp.Controllers;
using System;
using System.Collections;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShoeStoreApp.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShoeStoreAppContext _context;

        public ProductController(ILogger<HomeController> logger, ShoeStoreAppContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            List<Product> products = _context.Products.ToList();
            //ViewData["Products"] = products; 
            Combine combine = new Combine();
            combine.Products = products;
            var productList = _context.Products;
            int pageSize = 10;
            var paginatedProducts = await PaginatedList<Product>.CreateAsync(productList, pageNumber ?? 1, pageSize);
            combine.PaginatedListProduct = paginatedProducts;
            return PartialView("~/Views/Admin/Product.cshtml", combine);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/AddProduct", Name = "AddProduct")]
        public async Task<IActionResult> AddProduct([Bind("Name", "Price", "Description", "Brand", "Gender", "Category")] Product product, string[] color, IFormFile[] file)
        {
            if (ModelState.IsValid)
            {
                product.Variants = new List<ProductVariant>();
                for (int i = 0; i < color.Length; i++)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    var fileName = Path.Combine(uploadFolder, Guid.NewGuid().ToString() + Path.GetExtension(file[i].FileName));
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        file[i].CopyToAsync(fileStream);
                    }
                    ProductVariant newVarient = new ProductVariant
                    {
                        Color = String.Join("/", color[i]),
                        ImgPath = fileName,
                        Product = product,
                    };
                    product.Variants.Add(newVarient);
                }
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index");   
        }
    }
    public class Combine
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public PaginatedList<Product> PaginatedListProduct { get; set; }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}