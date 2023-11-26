using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeStoreApp.Data;
using ShoeStoreApp.Models;

namespace ShoeStoreApp.Controllers
{
    public class ProductVariantItemsController : Controller
    {
        private readonly ShoeStoreAppContext _context;

        public ProductVariantItemsController(ShoeStoreAppContext context)
        {
            _context = context;
        }

        // GET: ProductVariantItems
        public async Task<IActionResult> Index()
        {
            var shoeStoreDbContext = _context.ProductVariantItems.Include(p => p.ProductVariant);
            return View(await shoeStoreDbContext.ToListAsync());
        }

        // GET: ProductVariantItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductVariantItems == null)
            {
                return NotFound();
            }

            var productVariantItem = await _context.ProductVariantItems
                .Include(p => p.ProductVariant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariantItem == null)
            {
                return NotFound();
            }

            return View(productVariantItem);
        }

        // GET: ProductVariantItems/Create
        public IActionResult Create()
        {
            ViewData["ProductVariantId"] = new SelectList(_context.Set<ProductVariant>(), "Id", "Id");
            return View();
        }

        // POST: ProductVariantItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Size,StockQuantity,ProductVariantId")] ProductVariantItem productVariantItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productVariantItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductVariantId"] = new SelectList(_context.Set<ProductVariant>(), "Id", "Id", productVariantItem.ProductVariantId);
            return View(productVariantItem);
        }

        // GET: ProductVariantItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductVariantItems == null)
            {
                return NotFound();
            }

            var productVariantItem = await _context.ProductVariantItems.FindAsync(id);
            if (productVariantItem == null)
            {
                return NotFound();
            }
            ViewData["ProductVariantId"] = new SelectList(_context.Set<ProductVariant>(), "Id", "Id", productVariantItem.ProductVariantId);
            return View(productVariantItem);
        }

        // POST: ProductVariantItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Size,StockQuantity,ProductVariantId")] ProductVariantItem productVariantItem)
        {
            if (id != productVariantItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariantItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariantItemExists(productVariantItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductVariantId"] = new SelectList(_context.Set<ProductVariant>(), "Id", "Id", productVariantItem.ProductVariantId);
            return View(productVariantItem);
        }

        // GET: ProductVariantItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductVariantItems == null)
            {
                return NotFound();
            }

            var productVariantItem = await _context.ProductVariantItems
                .Include(p => p.ProductVariant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariantItem == null)
            {
                return NotFound();
            }

            return View(productVariantItem);
        }

        // POST: ProductVariantItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductVariantItems == null)
            {
                return Problem("Entity set 'ShoeStoreDbContext.ProductVariantItem'  is null.");
            }
            var productVariantItem = await _context.ProductVariantItems.FindAsync(id);
            if (productVariantItem != null)
            {
                _context.ProductVariantItems.Remove(productVariantItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariantItemExists(int id)
        {
          return (_context.ProductVariantItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
