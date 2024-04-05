using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Entities;
using MusicSystem.Helper;
using MusicSystem.Repository;
using MusicSystem.Repository.IRpository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//SE INYECTAN LAS INTERFACES
builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IComboHelper, ComboHelper>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//SE INYECTA LA CONEXION DE LA BASE DE DATOS AL PROGRAMA
builder.Services.AddDbContext<DataContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

//SE AGREGA PARA QUE LA APLICACION SEPA QUE YA TIENE USUARIO
//TODO: HACER LOS PASSWORD MAS SEGURO
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider; //ES EL GENERADOR DE TOKEN POR DEFECTO, SE PUEDE CREAR UNO
    cfg.SignIn.RequireConfirmedEmail = true; //LOS USUARIOS DEBEN SER CONFIRMADOS

    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    //cfg.Password.RequiredLength = 8;

    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //TIEMPO DE BLOQUEO DEL USUARIO
    cfg.Lockout.MaxFailedAccessAttempts = 3; //TRES INTENTOS Y SE BLOQUEAN
    cfg.Lockout.AllowedForNewUsers = true;//TODOS LOS USUARIOS SE BLOQUEAN

})
     .AddDefaultTokenProviders()//SE AGREGA POR DEFECTO EL TOKEN
    .AddEntityFrameworkStores<DataContext>();

var app = builder.Build();


SeedData(app);
//ESTA FUNCION REALIZA LA INYECCION MANUAL DEL SEEDDB
void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service.SeedAsync().Wait();
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); //PARA AGREGAR AUTENTICACIÓN
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
