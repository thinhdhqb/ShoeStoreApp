using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeStoreApp.Models;

namespace ShoeStoreApp.Data
{
    public class ShoeStoreAppContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantItem> ProductVariantItems { get; set; } = default!;


        public ShoeStoreAppContext (DbContextOptions<ShoeStoreAppContext> options)
            : base(options)
        {
        }

    }
}
