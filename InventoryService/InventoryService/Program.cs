using InventoryService.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryContext>(options =>
    options
        .UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=password")
        .UseSnakeCaseNamingConvention()
);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
