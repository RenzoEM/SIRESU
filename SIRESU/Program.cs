using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIRESU.Data;

var builder = WebApplication.CreateBuilder(args);

// Obtener cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configurar DbContext para Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Contexto personalizado SIRESU
builder.Services.AddDbContext<SiresuContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSession();

// Configurar Kestrel para usar puerto dinámico de Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// Inicializar datos por defecto (admin y cliente)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SiresuContext>();
    DbInitializer.Initialize(context);
}

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();
