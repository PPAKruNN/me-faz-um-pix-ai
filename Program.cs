using FazUmPix.Data;
using FazUmPix.Middlewares;
using FazUmPix.Policies;
using FazUmPix.Repositories;
using FazUmPix.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Services
builder.Services.AddScoped<KeysService>();

// Repository / Database
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<KeysRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PaymentProviderRepository>();
builder.Services.AddScoped<PaymentProviderAccountRepository>();

// Policies
builder.Services.AddScoped<KeysPolicies>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
