using ControleDespesas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=despesas.db"));

builder.Services.AddControllersWithViews();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔥 ESSA LINHA FALTAVA
app.MapRazorPages();

app.Run();