using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Database;

public class InventoryContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}
