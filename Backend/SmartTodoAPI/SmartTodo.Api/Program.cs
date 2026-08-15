
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartTodoAPI.Configurations;
using SmartTodoAPI.Data;
using SmartTodoAPI.Middleware;
using SmartTodoAPI.Repositories.Implementations;
using SmartTodoAPI.Repositories.Interfaces;
using SmartTodoAPI.Services.Implementations;
using SmartTodoAPI.Services.Interfaces;
using System.Text;

namespace SmartTodoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // SecurityDefinition means What authentication mechanism exists? here it is bearer
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement //SecurityRequirement-Which security mechanism should Swagger apply?
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
                        Array.Empty<string>()
                    }
                });
            });
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters { 
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),//The ! tells the nullable compiler:"I know this configuration value isn't null."
                ValidateIssuer=true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"], //Careful=> ValidateXXX ? Should I validate it? ? bool & ValidXXX    ? What value is valid?  ? string
                ValidateLifetime=true, //You don't need to manually specify the expiration time here. Because the JWT itself already contains:exp = expiration timestamp
                                       //Yes, your JWT contains the issuer and audience as well. But the API still needs to know what values it considers trustworthy.Because someone can create/modify a token and write:into the payload.
                }; // You now have the same secret on both sides: Generation , Validation That's the symmetric JWT concept.
            }); //this only tells ASP.NET Core: "Use JWT Bearer authentication.
                //options is the configuration object for the JWT Bearer authentication handler.Later we'll tell it:Use this SecretKey Use this Issuer,Use this Audience,Validate expiration,Validate signature
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}


