using GestionInventaire.BLL.Services;
using GestionInventaire.DAL.Data;
using GestionInventaire.DAL.Repositories;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;
using GestionInventaire.Web.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================
// CONFIGURATION SHORTCUTS
// ======================
var configuration = builder.Configuration;

var identityCfg = configuration.GetSection("Identity");

var cookieExpireMinutes =
    configuration.GetValue<int>("Cookie:ExpireMinutes", 30);

var seedCreateAdmin =
    configuration.GetValue<bool>("Seed:CreateAdmin", true);

var seedAdminEmail =
    configuration.GetValue<string>("Seed:AdminEmail", "admin@technologis.com");

var seedAdminPassword =
    configuration.GetValue<string>("Seed:AdminPassword", "Admin123!");

var seedRoles =
    configuration.GetSection("Seed:Roles").Get<string[]>()
    ?? new[] { "Admin", "Gestionnaire", "Technicien" };

// ======================
// DATABASE
// ======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection")));

// ======================
// IDENTITY
// ======================
builder.Services
    .AddIdentity<Utilisateur, IdentityRole>(options =>
    {
        options.Password.RequireDigit =
            identityCfg.GetValue<bool>("Password:RequireDigit", true);
        options.Password.RequiredLength =
            identityCfg.GetValue<int>("Password:RequiredLength", 6);
        options.Password.RequireUppercase =
            identityCfg.GetValue<bool>("Password:RequireUppercase", false);
        options.Password.RequireNonAlphanumeric =
            identityCfg.GetValue<bool>("Password:RequireNonAlphanumeric", false);

        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(
                identityCfg.GetValue<int>("Lockout:DefaultLockoutMinutes", 5));
        options.Lockout.MaxFailedAccessAttempts =
            identityCfg.GetValue<int>("Lockout:MaxFailedAccessAttempts", 5);

        options.User.RequireUniqueEmail =
            identityCfg.GetValue<bool>("UserRequireUniqueEmail", true);

        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ======================
// COOKIE CONFIG
// ======================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieExpireMinutes);
});

// ======================
// EMAIL SERVICE
// ======================
builder.Services.AddTransient<IEmailSender, MailjetEmailSender>();

// ======================
// REPOSITORIES (DAL)
// ======================
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IActifRepository, ActifRepository>();
builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddScoped<IAffectationRepository, AffectationRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IMouvementStockRepository, MouvementStockRepository>();
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IProduitRepository, ProduitRepository>();
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// ======================
// SERVICES (BLL)
// ── ServiceService fully qualified — évite le conflit avec System.ServiceProcess
// ======================
builder.Services.AddScoped<IActifService, ActifService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAffectationService, AffectationService>();
builder.Services.AddScoped<ICategorieService, CategorieService>();
builder.Services.AddScoped<IProduitService, ProduitService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IRapportService, RapportService>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IServiceService,
    GestionInventaire.BLL.Services.ServiceService>();

// ======================
// AUTOMAPPER
// ======================
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AffectationProfile>();
    cfg.AddProfile<DashboardProfile>();
    cfg.AddProfile<HomeProfile>();
    cfg.AddProfile<CategorieProfile>();
    cfg.AddProfile<ProduitProfile>();
    cfg.AddProfile<StockProfile>();
    cfg.AddProfile<EmployeProfile>();
    cfg.AddProfile<MaintenanceProfile>();
    cfg.AddProfile<AuditProfile>();
    cfg.AddProfile<ActifProfile>();
    cfg.AddProfile<RapportProfile>();
    cfg.AddProfile<UtilisateurProfile>();
    cfg.AddProfile<ServiceProfile>();
});

// ======================
// MVC & RAZOR PAGES
// ======================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ======================
// MIDDLEWARE
// ======================
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ======================
// SEED ROLES & ADMIN
// ======================
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Utilisateur>>();
    var dbContext = services.GetRequiredService<AppDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    await dbContext.Database.MigrateAsync();

    foreach (var role in seedRoles ?? Array.Empty<string>())
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
            logger.LogInformation("Rôle créé : {Role}", role);
        }
    }

    if (seedCreateAdmin)
    {
        var adminUser = await userManager.FindByEmailAsync(seedAdminEmail);
        if (adminUser == null)
        {
            var newAdmin = new Utilisateur
            {
                UserName = seedAdminEmail,
                Email = seedAdminEmail,
                Nom = "Administrateur",
                Prenom = "Système",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdmin, seedAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    newAdmin, seedRoles?.FirstOrDefault() ?? "Admin");
                logger.LogInformation("Admin créé : {Email}", seedAdminEmail);
            }
            else
            {
                foreach (var error in result.Errors)
                    logger.LogError(error.Description);
            }
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Erreur initialisation BD");
    if (app.Environment.IsDevelopment()) throw;
}

// ======================
// ROOT REDIRECTION
// ← DOIT être AVANT MapRazorPages / MapControllerRoute
// ======================
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
            context.Response.Redirect("/Identity/Account/Login");
        else
            context.Response.Redirect("/Home");
        return;
    }
    await next();
});

// ======================
// ROUTING
// ======================
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();