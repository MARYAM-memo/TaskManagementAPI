using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Data.Mapping;
using TodoAPI.DataAccess;
using TodoAPI.Extensions;
using TodoAPI.Interfaces;
using TodoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
var DatabaseCS = builder.Configuration.GetConnectionString("DatabaseCS");
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseNpgsql(DatabaseCS));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>().AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddJwtConfiguration(builder);
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddSwaggerWithJwtAuth();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProfileMapping>(), typeof(ProfileMapping).Assembly);
builder.Services.AddFluentValidatore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

await app.Services.AddScopeToUserAndRole(); //extension

app.MapControllers();

app.Run();
