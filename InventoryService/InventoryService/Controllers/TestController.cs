using InventoryService.Objects;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpPost]
    public async Task<int> OrderInParallel()
    {
        const string productName = "Cheese";
        var client = new HttpClient();
        var order = new ProductOrder { Quantity = 1 };
        var orderProductUri = new Uri($"http://localhost:5266/inventory/{productName}");

        var tasks = Enumerable.Range(0, 100).Select(_ => Task.Factory.StartNew(async () => await client.PatchAsJsonAsync(orderProductUri, order)));

        await Task.WhenAll(tasks);

        var checkStockUri = $"http://localhost:5266/inventory/{productName}";

        return await client.GetFromJsonAsync<int>(checkStockUri);
    }
}
