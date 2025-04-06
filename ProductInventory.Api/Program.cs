using Microsoft.EntityFrameworkCore;
using ProductInventory.Api.Data;
using ProductInventory.Api.Data.Helpers;
using ProductInventory.Api.Middlewares;
using ProductInventory.Api.Repositories;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Information("Starting up...");

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "AllowBlazorClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseInMemoryDatabase("ProductInventoryDb"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Host.UseSerilog();

var app = builder.Build();

DbSeeder.SeedFromJson(app);

app.UseCors(corsPolicy);
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductInventory API v1"));
app.MapOpenApi();
app.UseMiddleware<LoggingMiddleware>();
app.UseRouting();
app.MapControllers();
app.UseDefaultFiles();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();