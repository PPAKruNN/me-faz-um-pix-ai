using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.MapControllers();

// Expose metrics
app.MapMetrics(); 

app.Run();
