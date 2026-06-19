using InventoryService.Database;
using InventoryService.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Filters;

public class ValidateProductOrderFilter(InventoryContext _context) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var order = context.ActionArguments.Values.OfType<ProductOrder>().FirstOrDefault();

        if (order is null)
        {
            context.Result = new BadRequestObjectResult("ProductOrder is required.");
            return;
        }

        var productName = context.ActionArguments["product"] as string;

        var product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Name == productName);
        if (product is null)
        {
            context.Result = new BadRequestObjectResult($"Requested product {productName} does not exist.");
            return;
        }

        if (order.Quantity > product.StockQuantity)
        {
            context.Result = new BadRequestObjectResult("Order quantity cannot exceed product stock quantity.");
            return;
        }

        await next();
    }
}
