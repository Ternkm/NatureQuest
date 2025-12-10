using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NatureQuest.Data;
using NatureQuest.Services;
using NatureQuest.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add MVC and register filters
builder.Services.AddControllersWithViews();

// Register services
builder.Services.AddScoped<SpeciesService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ObservationService>();

// Register filters with DI
builder.Services.AddScoped<ObservationMappingFilter>();

// Drop down filter
builder.Services.AddScoped<DropdownPopulateFilter>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Apply migrations and seed roles/users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    // db.Database.Migrate(); // Uncomment if you want automatic migrations

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    SeedRolesAsync(roleManager, userManager).GetAwaiter().GetResult();
}

static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    string[] roleNames = { "Admin", "Guest" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Admin user
    var adminEmail = "admin@NatureQuest.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (!createResult.Succeeded)
            foreach (var e in createResult.Errors)
                Console.WriteLine($"[Seed] Admin create error: {e.Code} - {e.Description}");
    }
    await userManager.AddToRoleAsync(adminUser, "Admin");

    // Guest user
    var guestEmail = "guest@NatureQuest.com";
    var guestUser = await userManager.FindByEmailAsync(guestEmail);
    if (guestUser == null)
    {
        guestUser = new IdentityUser { UserName = guestEmail, Email = guestEmail, EmailConfirmed = true };
        var createResult = await userManager.CreateAsync(guestUser, "GuestPass123!");
        if (!createResult.Succeeded)
            foreach (var e in createResult.Errors)
                Console.WriteLine($"[Seed] Guest create error: {e.Code} - {e.Description}");
    }
    await userManager.AddToRoleAsync(guestUser, "Guest");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
