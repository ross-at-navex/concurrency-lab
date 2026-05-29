namespace InventoryService.Models;

public class Product
{
    public int ProductId { get; set; }
    public required string NameFoo { get; set; }
    public int StockQuantity { get; set; }
    public int RowVersion { get; set; }
    public DateTime LastUpdated { get; set; }
}
