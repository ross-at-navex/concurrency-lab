using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Database;

public class InventoryContext(DbContextOptions<InventoryContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
