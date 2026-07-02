namespace InventoryService.Locking;

public interface ISemaphoreSlimWrapper
{
    int Release();
    Task WaitAsync();
}
