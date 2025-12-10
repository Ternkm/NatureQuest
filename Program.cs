using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NatureQuest.Data;
using NatureQuest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SpeciesService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ObservationService>();
builder.Services.AddRazorPages();

//Fresh baked Cookies!
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;                  
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.Cookie.SameSite = SameSiteMode.Strict;   
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
    options.SlidingExpiration = true;                
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


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

// Apply migrations and seed roles/users in one scope
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    //db.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    SeedRolesAsync(roleManager, userManager).GetAwaiter().GetResult();
}


static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    // Ensure roles exist
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
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (!createResult.Succeeded)
        {
            foreach (var e in createResult.Errors)
                Console.WriteLine($"[Seed] Admin create error: {e.Code} - {e.Description}");
        }
    }

    var adminRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
    if (!adminRoleResult.Succeeded)
    {
        foreach (var e in adminRoleResult.Errors)
            Console.WriteLine($"[Seed] Admin role assign error: {e.Code} - {e.Description}");
    }

    // Guest user
    var guestEmail = "guest@NatureQuest.com";
    var guestUser = await userManager.FindByEmailAsync(guestEmail);
    if (guestUser == null)
    {
        guestUser = new IdentityUser
        {
            UserName = guestEmail,
            Email = guestEmail,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(guestUser, "GuestPass123!");
        if (!createResult.Succeeded)
        {
            foreach (var e in createResult.Errors)
                Console.WriteLine($"[Seed] Guest create error: {e.Code} - {e.Description}");
        }
    }

    var guestRoleResult = await userManager.AddToRoleAsync(guestUser, "Guest");
    if (!guestRoleResult.Succeeded)
    {
        foreach (var e in guestRoleResult.Errors)
            Console.WriteLine($"[Seed] Guest role assign error: {e.Code} - {e.Description}");
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
