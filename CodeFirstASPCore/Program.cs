using CodeFirstASPCore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("StudentDBContext") ?? throw new InvalidOperationException("Connection string 'StudentDBContext' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

//Registering Connection String
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetService<IConfiguration>();
builder.Services.AddDbContext<StudentDBContext>(item => item.UseSqlServer(config.GetConnectionString("ASPNETCORECONNECTIONSTRING")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
