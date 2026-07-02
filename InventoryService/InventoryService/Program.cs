using InventoryService.Database;
using InventoryService.Filters;
using InventoryService.Locking;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryContext>(options =>
    options
        .UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=password")
        .UseSnakeCaseNamingConvention()
);

builder.Services.AddControllers();
builder.Services.AddScoped<ValidateProductOrderFilter>();
builder.Services.AddScoped<ValidateProductCreateRequestFilter>();
builder.Services.AddScoped<LockFilter>();
builder.Services.AddSingleton<ISemaphoreSlimWrapper, SemaphoreSlimWrapper>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
