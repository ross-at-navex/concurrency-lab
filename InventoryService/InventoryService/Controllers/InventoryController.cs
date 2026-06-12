using InventoryService.Database;
using InventoryService.Filters;
using InventoryService.Models;
using InventoryService.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController(InventoryContext _context) : ControllerBase
{
    [HttpGet("{product}")]
    public async Task<int> GetInventoryAsync(string product)
    {
        var item = await _context.Products.SingleAsync(p => p.Name == product);
        return item.StockQuantity;
    }

    [HttpPatch]
    [Route("{product}")]
    [ServiceFilter(typeof(ValidateProductOrderFilter))]
    public async Task<int> UpdateInventoryAsync(string product, ProductOrder order)
    {
        var item = await _context.Products.SingleAsync(p => p.Name == product);
        item.StockQuantity -= order.Quantity;
        item.LastUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return item.StockQuantity;
    }

    // TODO: return a ProductCreateResponse instead of the Product entity directly
    // TODO: add exception handling middleware
    [HttpPost]
    [ServiceFilter(typeof(ValidateProductCreateRequestFilter))]
    public async Task<Product> CreateProductAsync(ProductCreateRequest productToCreate)
    {
        var product = new Product
        {
            Name = productToCreate.Name,
            StockQuantity = productToCreate.StockQuantity,
            LastUpdated = DateTime.UtcNow
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
