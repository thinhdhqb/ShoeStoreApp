using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoeStoreApp.Configs;
using ShoeStoreApp.Data;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<ShoeStoreDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ShoeStoreDbContext") ?? throw new InvalidOperationException("Connection string 'ShoeStoreDbContext' not found.")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}