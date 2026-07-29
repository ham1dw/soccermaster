using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddScoped<soccer.Services.IFileService, soccer.Services.FileService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
    options.Events.OnRedirectToLogin = context =>
    {
        Console.WriteLine($"[AUTH] Qeyri-avtorize olunmus sorgu Login-e yonlendirilir: {context.Request.Path}");
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});


var app = builder.Build();

Console.WriteLine("[STARTUP] App builder tamamlandi, host qurulur...");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Apply pending EF Core migrations automatically and seed admin user/roles.
// Any failure here is logged loudly to the console AND the logger, so the
// cause is always visible instead of the app silently doing nothing.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var config = services.GetRequiredService<IConfiguration>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        Console.WriteLine("[STARTUP] Verilenler bazasina qosulur ve migrasiyalar tetbiq edilir...");
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        Console.WriteLine("[STARTUP] Migrasiyalar tamamlandi.");

        Console.WriteLine("[STARTUP] Admin istifadeci/rol yoxlanilir...");
        await AdminSeeder.SeedAdminAsync(services, config);
        Console.WriteLine("[STARTUP] Admin seed emeliyyati tamamlandi.");
    }
    catch (Exception ex)
    {
        // Print to console immediately so it can never be missed, in addition
        // to the structured logger.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[STARTUP HATASI] Verilenler bazasi ile elaqe/seed zamani xeta:");
        Console.WriteLine(ex.ToString());
        Console.ResetColor();
        logger.LogError(ex, "An error occurred applying migrations or seeding the DB.");
    }
}

app.UseHttpsRedirection();
app.MapStaticAssets(); 

app.UseRouting();
// DEV ONLY: automatically allow access to Admin area without interactive login.
// This middleware injects a development user with Admin role when running in Development.
// REMOVE THIS BEFORE DEPLOYING TO PRODUCTION.
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "dev-admin"),
                new Claim(ClaimTypes.Email, "admin@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "Development");
            context.User = new ClaimsPrincipal(identity);
        }
        await next();
    });
}
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

// Area route (must be registered before default)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();