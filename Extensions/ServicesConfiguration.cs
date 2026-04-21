using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TodoAPI.Data.Seed;
using TodoAPI.Settings;
using TodoAPI.Validators.Task;

namespace TodoAPI.Extensions;

public static class ServicesConfiguration
{
      public static void AddJwtConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
      {
            var jwtString = builder.Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtString);
            builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
            var jwtSettings = jwtString.Get<JwtSettings>();
            byte[] key = jwtSettings != null ? Encoding.ASCII.GetBytes(jwtSettings.Secret) : [];

            services.AddAuthentication(opt =>
            {
                  opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                  opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                  opt.SaveToken = true;
                  opt.RequireHttpsMetadata = false;
                  opt.TokenValidationParameters = new TokenValidationParameters
                  {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAudience = jwtSettings?.Audience,
                        ValidIssuer = jwtSettings?.Issuer,
                        ClockSkew = TimeSpan.Zero,
                  };
            });

      }

      public static void AddSwaggerWithJwtAuth(this IServiceCollection services)
      {
            services.AddSwaggerGen(
                  opt =>
                  {
                        var bearerScheme = new OpenApiSecurityScheme
                        {
                              Type = SecuritySchemeType.Http,
                              Scheme = "bearer",
                              BearerFormat = "JWT",
                              In = ParameterLocation.Header,
                              Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
                              Name = "Authorization",
                        };

                        opt.AddSecurityDefinition("Bearer", bearerScheme);

                        opt.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                        {
                              {new OpenApiSecuritySchemeReference("Bearer", doc),
                              new List<string>()
                              }
                        });
                  }
            );
      }

      public static async Task AddScopeToUserAndRole(this IServiceProvider appServices)
      {
            using var scope = appServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                  await SeedData.Initialize(services);
            }
            catch (Exception ex)
            {
                  var logger = services.GetRequiredService<ILogger<Program>>();
                  logger.LogError(ex, "An error occurred while seeding the database.");
            }
      }

      public static void AddFluentValidatore(this IServiceCollection services)
      {
            services.AddFluentValidationAutoValidation(config =>
            {
                  config.DisableDataAnnotationsValidation = true;
            });
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<TaskRequestValidator>();
      }

}
