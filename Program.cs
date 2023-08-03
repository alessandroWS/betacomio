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
global using betacomio.ModelsAdventureWorksLT2019;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<DataContext2>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));
    builder.Services.AddDbContext<AdventureWorksLt2019Context>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme{
        Description = """Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOldOrderService, OldOrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:4200") // Sostituisci con l'URL del frontend Angular
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseCors();
//
app.Run();