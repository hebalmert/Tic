using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using Tic.Shared.Entites;
using Tic.Web.Data;
using Tic.Web.Helpers;
using Tic.Web.LoadCountries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Se Agrega el .AddJsonOption para evitar todas las redundancias Ciclicas automaticamente
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Para Manejo de Variables de Session
builder.Services.AddSession();

//Conexion de la Base de Datos
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=DefaultConnection"));

//Para Guardar las Cookies de Validacion
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    option =>
    {
        option.LoginPath = "/Account/Login";
        option.AccessDeniedPath = "/Account/Login";
    });

//Envio hacia el sistema de No Autorizados
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/NotAuthorized";
    options.AccessDeniedPath = "/Account/NotAuthorized";
});

//Para realizar logueo de los usuarios
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    //Agregamos Validar Correo para dar de alta al Usuario
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;

    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    //Sistema para bloquear por 5 minutos al usuario por intento fallido
    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  //TODO: Cambiar a 5 minutos
    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.AllowedForNewUsers = true;
}).AddDefaultTokenProviders()  //Complemento Validar Correo
  .AddEntityFrameworkStores<DataContext>();

//Implementacion del sistema de JWT y Validacion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

//Sistema de Notficacion Toast
builder.Services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

// Configurar servicios
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-US");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("es-US") };
    options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("es-US") };
});

//Inicio de Area de los Serviciios
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        /*builder.WithOrigins("https://localhost:7105")*/ // dominio de tu aplicación Blazor
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
               .WithExposedHeaders(new string[] { "Totalpages", "conteo" }); //Variables que se enviaran por le Headers
    });
});

//Inyeccion de Dependencias
builder.Services.AddTransient<SeedDb>();
//Inyeccion de AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IComboHelper, ComboHelper>();
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IFileStorage, FileStorage>();






var app = builder.Build();

//Implementacion del SeedDb para llenado de Base de datos y Otros
SeedDb(app);
void SeedDb(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedDbAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middleware para manejar errores 404
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

//Para el Manejo de Toast Mensajes
app.UseNotyf();

//Llamar el Servicio de CORS
app.UseCors();


app.UseHttpsRedirection();

app.UseStaticFiles();

//Para Implementar la Autoricacion en Controladores y Layout
app.UseAuthorization();

//Para Usar las Variables de Session
//Complemetno del Services Arriba
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
