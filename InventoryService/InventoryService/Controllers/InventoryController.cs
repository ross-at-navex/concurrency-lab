using InventoryService.Database;
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
    public async Task<int> UpdateInventoryAsync(string product, ProductOrder order)
    {
        var item = await _context.Products.SingleAsync(p => p.Name == product);

        if (order.Quantity > item.StockQuantity)
        {
            throw new InvalidOperationException("Order quantity exceeds existing product stock");
        }

        item.StockQuantity -= order.Quantity;
        item.LastUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return item.StockQuantity;
    }

    // TODO: return a ProductCreateResponse instead of the Product entity directly
    // TODO: add validation middleware
    // TODO: add exception handling middleware
    [HttpPost]
    public async Task<Product> CreateProductAsync(ProductCreateRequest productToCreate)
    {
        if (productToCreate.StockQuantity < 0)
        {
            throw new InvalidOperationException("Cannot create a product with an inventory less than 0");
        }

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
