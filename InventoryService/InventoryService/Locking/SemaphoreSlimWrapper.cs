namespace InventoryService.Locking;

public class SemaphoreSlimWrapper : ISemaphoreSlimWrapper
{
    readonly SemaphoreSlim _sem = new(1, 1);

    public Task WaitAsync() => _sem.WaitAsync();

    public int Release() => _sem.Release();
}
