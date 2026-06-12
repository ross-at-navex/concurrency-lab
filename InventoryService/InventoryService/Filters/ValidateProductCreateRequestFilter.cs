using InventoryService.Database;
using InventoryService.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventoryService.Filters;

public class ValidateProductCreateRequestFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.ActionArguments.Values.OfType<ProductCreateRequest>().FirstOrDefault();

        if (request is null)
        {
            context.Result = new BadRequestObjectResult("ProductCreateRequest is required.");
            return;
        }

        if (request.StockQuantity < 0)
        {
            context.Result = new BadRequestObjectResult("Quantity cannot be less than 0.");
            return;
        }

        await next();
    }
}
