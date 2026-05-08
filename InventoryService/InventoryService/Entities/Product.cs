namespace InventoryService.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int StockQuantity { get; set; }
    public int RowVersion { get; set; }
    public DateTime LastUpdated { get; set; }
}
