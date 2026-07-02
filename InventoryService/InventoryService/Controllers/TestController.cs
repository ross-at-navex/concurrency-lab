using InventoryService.Objects;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController() : ControllerBase
{
    [HttpPost]
    public async Task<int> OrderInParallel()
    {
        const string productName = "Cheese";
        var client = new HttpClient();
        var order = new ProductOrder { Quantity = 1 };
        var orderProductUri = new Uri($"http://localhost:5266/inventory/{productName}");

        var tasks = Enumerable.Range(0, 100).Select(async _ =>
        {
            var ret = await client.PatchAsJsonAsync(orderProductUri, order);
            return ret;
        });

        var statusCodes = await Task.WhenAll(tasks);

        var failedTasks = statusCodes.Where(t => t.StatusCode != System.Net.HttpStatusCode.OK).ToList();

        if (failedTasks.Count != 0)
        {
            var messagesTasks = failedTasks.Select(async ft => await ft.Content.ReadAsStringAsync());
            var messages = await Task.WhenAll(messagesTasks);
            throw new Exception("One or more tasks threw an exception");
        }

        var checkStockUri = $"http://localhost:5266/inventory/{productName}";

        return await client.GetFromJsonAsync<int>(checkStockUri);
    }
}
