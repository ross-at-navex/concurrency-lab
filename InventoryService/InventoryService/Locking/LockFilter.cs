using Microsoft.AspNetCore.Mvc.Filters;

namespace InventoryService.Locking;

public class LockFilter(ISemaphoreSlimWrapper _sem) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await _sem.WaitAsync();
        try
        {
            await next();
        }
        finally
        {
            _sem.Release();
        }
    }
}
