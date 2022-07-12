using Microsoft.EntityFrameworkCore;
using MvcExample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ExampleContext>(
    opt => opt.UseSqlServer("Server=DESKTOP-2P4P6M3;Database=Example;Trusted_Connection=True;")
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Articles}/{action=Index}/{id?}");

app.Run();
