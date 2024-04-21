global using ProdavnicaTehnikeBekend.Models;
using Microsoft.AspNetCore.Hosting;
using ProdavnicaTehnikeBekend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using ProdavnicaTehnikeBekend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ubiety.Dns.Core;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard ",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ProdavnicaTehnikeContext>();


builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


builder.Services.AddScoped<IKupacRepository, KupacRepository>();
builder.Services.AddScoped<IZaposleniRepository, ZaposleniRepository>();
builder.Services.AddScoped<IProizvodRepository, ProizvodRepository>();
builder.Services.AddScoped<IPorudzbinaRepository, PorudzbinaRepository>();
builder.Services.AddScoped<IPorudzbinaProizvodRepository, PorudzbinaProizvodRepository>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });
builder.Services.AddScoped<IAutentifikacijaRepository, AutentifikacijaRepository>();
builder.Services.AddSingleton(new JwtHelper(builder.Configuration));
builder.Services.AddScoped<HashingService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
