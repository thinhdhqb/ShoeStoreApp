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
using Microsoft.IdentityModel.Logging;

namespace ShoeStoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                    productList = productList.OrderBy(s => s.Brand);
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
            return PartialView(combine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            Product product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProducts(string productSelectedIds)
        {
            string[] productIds = productSelectedIds.Split(',');
            int[] intProductIds = productIds.Select(int.Parse).ToArray();
            foreach (int id in intProductIds)
            {
                _context.Products.Remove(_context.Products.Find(id));
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Product product = _context.Products.Find(id);
            List<ProductVariant> productVariants = _context.ProductVariants.Where(
                    c => c.ProductId == id).Include(c => c.Items).ToList();
            CombineEditProduct combine = new CombineEditProduct
            {
                Product = product,
                ProductVariants = productVariants,
            };
            return PartialView(combine);
        }
        [HttpPost]
        public async Task<IActionResult> PostEditProduct([FromRoute] int id, 
            [Bind("Name", "Price", "Description", "Brand", "Gender", "Category")] Product product, 
            string[] color, 
            IFormFile[] file, 
            string[] size, 
            string saveProductEditImage)
        {
            //Change product
            Product productToEdit = _context.Products.Include(c => c.Variants).Where(c => c.Id == id).ToList()[0];
            productToEdit.Name = product.Name;
            productToEdit.Description = product.Description;
            productToEdit.Price = product.Price;
            productToEdit.Brand = product.Brand;
            productToEdit.Gender = product.Gender;
            productToEdit.Category = product.Category;
            _context.SaveChanges();
            //Change productVariant
            List<ProductVariant> productVariants = productToEdit.Variants.ToList();
            int count = 0;
            int imgCount = 0;
            foreach(var variant in productVariants) 
            {
                variant.Color = color[count];
                if (file.Length != 0) 
                {
                    string[] numberOfImgPathEdits = { };
                    if (saveProductEditImage != "")
                    {
                        numberOfImgPathEdits = saveProductEditImage.Substring(0, saveProductEditImage.Length - 3).Split(" / ");
                    }
                    int[] intVariantChangeImage = numberOfImgPathEdits.Select(int.Parse).ToArray();
                    Array.Sort(intVariantChangeImage);
                    if (intVariantChangeImage.Contains(count + 1))
                    {
                        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        var fileNameStore = Guid.NewGuid().ToString();
                        var fileName = Path.Combine(uploadFolder, fileNameStore + Path.GetExtension(file[imgCount].FileName));
                        using (var fileStream = new FileStream(fileName, FileMode.Create))
                        {
                            await file[imgCount].CopyToAsync(fileStream);
                        }
                        variant.ImgPath = "/images/" + fileNameStore + Path.GetExtension(file[imgCount].FileName);
                        imgCount++;
                    }
                }
                _context.SaveChanges();
                // Change product variant size
                ProductVariant variantSeleted = _context.ProductVariants.Include(c => c.Items)
                    .Where(c => c.Id == variant.Id).ToList()[0];
                List<ProductVariantItem> items = variantSeleted.Items.ToList();
                foreach (ProductVariantItem item in items) 
                {
                    string stockQuantityBySize = size[count];
                    Dictionary<string, int> productVariantItems = ParseInputString(stockQuantityBySize);
                    item.StockQuantity = productVariantItems[item.Size];
                    _context.SaveChanges();
                }
                count++;
                
            }
            return RedirectToAction("Index");
        }
        static Dictionary<string, int> ParseInputString(string input)
        {
            string[] pairs = input.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            foreach (string pair in pairs)
            {
                string[] parts = pair.Trim().Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int value))
                {
                    keyValuePairs.Add(parts[0], value);
                }
            }
            return keyValuePairs;
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
    public class CombineEditProduct
    {
        public Product Product { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }   

    }
}