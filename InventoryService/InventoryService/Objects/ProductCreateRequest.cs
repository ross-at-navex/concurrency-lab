namespace InventoryService.Objects;

public class ProductCreateRequest
{
    public required string Name { get; set; }
    public int StockQuantity { get; set; }
}
