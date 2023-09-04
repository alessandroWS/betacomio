global using betacomio.Services.OrderService;
global using betacomio.Models;
global using betacomio.Dtos.Order;
global using Microsoft.EntityFrameworkCore;
global using betacomio.Data;
global using AutoMapper;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.Security.Claims;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.Filters;
global using betacomio.ModelsAdventureWorksLT2019;
global using betacomio.Services.ProductServices;
global using betacomio.Services.OldOrderService;
global using betacomio.Dtos.OldOrder;
global using betacomio.Dtos.Product;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using betacomio.Dtos.User;
global using betacomio.Services.AdminRequestService;
global using NLog;
using betacomio.Services.LikesService;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using betacomio.Services.ProductCategoryService;
//global using betacomio.Dtos.AdminRequest;
// Creazione dell'istanza del WebApplicationBuilder



var builder = WebApplication.CreateBuilder(args);


// Aggiunge i servizi al container.

// Configurazione del DataContext con una connessione SQL Server per la DefaultConnection
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurazione del DataContext2 con una connessione SQL Server per la UserConnection
builder.Services.AddDbContext<DataContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));

// Configurazione del DataContext per AdventureWorksLt2019Context con una connessione SQL Server per AdventureConnection
builder.Services.AddDbContext<AdventureWorksLt2019Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureConnection")));

builder.Services.AddControllers().AddJsonOptions(jsOpt => jsOpt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Aggiunge i servizi del controller
builder.Services.AddControllers();

// Aggiunge la documentazione Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    // Aggiunge una definizione di sicurezza per l'autenticazione OAuth2 (usata per il Bearer Token)
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        // Descrizione dell'header di autorizzazione standard che usa lo schema Bearer
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",

        // Specifica il parametro in cui l'header di autorizzazione sarà incluso
        In = ParameterLocation.Header,

        // Specifica il nome del parametro
        Name = "Authorization",

        // Specifica il tipo di schema di sicurezza (ApiKey per il Bearer Token)
        Type = SecuritySchemeType.ApiKey
    });

    // Aggiunge un filtro per i requisiti di sicurezza alle operazioni Swagger
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configurazione dell'AutoMapper, specificando l'assembly che contiene i profili di mappatura
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Aggiunge i servizi con scopo (scoped) necessari al container
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOldOrderService, OldOrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAdminRequestService, AdminRequestService>();
builder.Services.AddScoped<ILikesService, LikesService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();


// Aggiunge l'autenticazione con JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            // Specifica la chiave simmetrica utilizzata per la firma e verifica dei token
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),

            // Non valida l'emittente del token (issuer)
            ValidateIssuer = false,

            // Non valida l'audience del token (destinatario)
            ValidateAudience = false
            
        };
    });

// Aggiunge l'accesso al contesto HTTP
builder.Services.AddHttpContextAccessor();

// Configura le politiche CORS per consentire richieste solo dall'URL specificato
builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(
                    build =>
                    {
                        //build.WithOrigins("https://localhost:7286/")
                        build.SetIsOriginAllowed(origin => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

builder.Services.AddDistributedMemoryCache(); // Usa una cache in memoria per scopi di esempio. In produzione, dovresti utilizzare una cache distribuita.

// Configura la sessione
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(
        builder.Configuration.GetValue<int>("SessionTimeout"));
    option.Cookie.IsEssential = true;
});

// Crea l'istanza dell'applicazione
var app = builder.Build();

// Configura la pipeline delle richieste HTTP

// In modalità di sviluppo, abilita Swagger e la UI di Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Reindirizza le richieste HTTP a HTTPS
app.UseHttpsRedirection();

// Abilita l'autenticazione
app.UseAuthentication();

// Abilita l'autorizzazione
app.UseAuthorization();

// Mappa i controller
app.MapControllers();

// Configura l'uso delle politiche CORS
app.UseCors();
app.UseSession();
// Avvia l'applicazione
app.Run();

//modifica fatta da Serena