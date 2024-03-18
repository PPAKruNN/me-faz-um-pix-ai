using FazUmPix.Authentication;
using FazUmPix.Data;
using FazUmPix.Middlewares;
using FazUmPix.Policies;
using FazUmPix.Repositories;
using FazUmPix.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

// Services
builder.Services.AddScoped<KeysService>();
builder.Services.AddScoped<PaymentsService>();
builder.Services.AddScoped<PaymentsProcessingService>();

builder.Services.AddSingleton<PaymentsProcessingService>();
// Pelo que pesquisei, deve-se ter apenas 1 conexão por app.
// Logo, já deixei como singleton pra não criar 1 conexão POR REQUEST.

// Repository / Database
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<KeysRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PaymentProviderRepository>();
builder.Services.AddScoped<PaymentProviderAccountRepository>();
builder.Services.AddScoped<PaymentsRepository>();

// Policies
builder.Services.AddScoped<KeysPolicies>();

// Authentication
builder.Services.AddAuthentication("BearerAuthentication")
  .AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("BearerAuthentication", null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
  options =>
  {
    options.SwaggerDoc("v1", new() { Title = "FazUmPix", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer",
      In = ParameterLocation.Header,
      Description = "Bearer Authorization header"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
  }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Collect metrics from app 
app.UseMetricServer();
app.UseHttpMetrics(options =>
{
  options.AddCustomLabel("host", context => context.Request.Host.Host);
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

// Expose metrics
app.MapMetrics();

app.Run();
