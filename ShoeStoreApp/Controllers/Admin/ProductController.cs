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
using System.Drawing.Printing;

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
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["BrandSortParm"] = sortOrder == "brand_asc" ? "brand_desc" : "brand_asc";
            ViewData["GenderSortParm"] = sortOrder == "gender_asc" ? "gender_desc" : "gender_asc";
            ViewData["CategorySortParm"] = sortOrder == "category_asc" ? "category_desc" : "category_asc";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var productList = from s in _context.Products select s;
            List<Product> products = _context.Products.ToList();
            Combine combine = new Combine();
            combine.Products = products;
            if (!String.IsNullOrEmpty(searchString))
            {
                productList = _context.Products.Where(s => s.Name.Contains(searchString)); 
            }
            switch (sortOrder)
            {
                case "price_desc":
                    productList = productList.OrderByDescending(s => s.Price);
                    break;
                case "Price":
                    productList = productList.OrderBy(s => s.Price);
                    break;
                case "brand_asc":
                    productList = productList.OrderBy(s =>s.Brand);
                    break;
                case "brand_desc":
                    productList = productList.OrderByDescending(s => s.Brand);
                    break;
                case "gender_asc":
                    productList = productList.OrderBy(s => s.Gender);
                    break;
                case "gender_desc":
                    productList = productList.OrderByDescending(s => s.Gender);
                    break;
                case "category_asc":
                    productList = productList.OrderBy(s => s.Category);
                    break;
                case "category_desc":
                    productList = productList.OrderByDescending(s => s.Category);
                    break;
            }
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
                    var fileNameStore = Guid.NewGuid().ToString();
                    var fileName = Path.Combine(uploadFolder, fileNameStore + Path.GetExtension(file[i].FileName));
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await file[i].CopyToAsync(fileStream);
                    }
                    ProductVariant newVarient = new ProductVariant
                    {
                        Color = String.Join("/", color[i]),
                        ImgPath = "/images/" + fileNameStore + Path.GetExtension(file[i].FileName),
                        Product = product,
                    };
                    product.Variants.Add(newVarient);
                }
                _context.Products.Add(product);
                _context.SaveChanges();
                List<ProductVariant> variants = product.Variants.ToList();
                foreach (ProductVariant variant in variants)
                {
                    for (int j = 36; j <= 45; j++)
                    {
                        ProductVariantItem newVariantItem = new ProductVariantItem
                        {
                            Size = j.ToString(),
                            StockQuantity = 10,
                            ProductVariantId = variant.Id
                        };
                        _context.ProductVariantItems.Add(newVariantItem);
                        _context.SaveChanges();
                    }
                }
            }
            
            return RedirectToAction("Index");   
        }
        [HttpPost]
        [Route("/DeleteProduct/{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            Product product = _context.Products.Find(id);
            if (product!=null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("/DeleteProducts", Name = "DeleteProducts")]
        public async Task<IActionResult> DeleteProducts(string productSelectedIds)
        {
            string[] productIds = productSelectedIds.Split(',');
            int[] intProductIds = productIds.Select(int.Parse).ToArray();
            foreach(int id in intProductIds)
            {
                _context.Products.Remove(_context.Products.Find(id));
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