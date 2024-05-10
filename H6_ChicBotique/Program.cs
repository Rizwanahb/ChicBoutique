using H6_ChicBotique.Authorization;
using H6_ChicBotique.Database;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
private static void Main(string[] args){

var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAny",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        // Add services to the container.
        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();

        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IUserService, UserService>();

        builder.Services.AddTransient<IAccountInfoRepository, AccountInfoRepository>();


        builder.Services.AddTransient<IHomeAddressRepository, HomeAddressRepository>();
        builder.Services.AddTransient<IHomeAddressService, HomeAddressService>();

        builder.Services.AddTransient<IAccountInfoService, AccountInfoService>();
        builder.Services.AddTransient<IAccountInfoRepository, AccountInfoRepository>();

        builder.Services.AddTransient<IPasswordEntityRepository, PasswordEntityRepository>();

        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();


        builder.Services.AddTransient<IShippingDetailsRepository, ShippingDetailsRepository>();
        builder.Services.AddTransient<IShippingDetailsService, ShippingDetailsService>();

        builder.Services.AddTransient<IPasswordEntityRepository, PasswordEntityRepository>();

        builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
        builder.Services.AddSingleton<IStockHandlerService, StockHandlerService>();

        builder.Services.AddScoped<IJwtUtils, JwtUtils>();

        builder.Services.AddDbContext<ChicBotiqueDatabaseContext>(
                        o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


        //builder.Services.AddSingleton<IStockHandlerService, StockHandlerService>();
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); // henter appsettings fra json 

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(
        c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChicBotique.API", Version = "v1" });
            // To Enable authorization using Swagger (JWT)  
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                        });
        });



        var app = builder.Build();
        app.UseHttpsRedirection();

        app.UseCors(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();

        app.UseAuthorization();
        //JWT middleware setup, use as replacement for  default Authorization
        app.UseMiddleware<JwtMiddleware>();
        app.MapControllers();

        app.Run();
    }
}