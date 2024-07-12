using DotnetStack.TodoApi.DataAccess;

namespace DotnetStack.TodoApi;

public class ToDoService(ToDoRepository repository, CacheService cacheService)
{
    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        return await cacheService.GetOrAdd($"ToDoItem:all", 
            async () => await repository.GetAll(), 30);
    }

    public async Task<ToDoItem> GetByIdAsync(string id)
    {
        return await cacheService.GetOrAdd($"ToDoItem:{id}", 
            async () => await repository.GetById(id), 30);
    }

    public async Task CreateAsync(ToDoItem item)
    {
        item.Id ??= Guid.NewGuid().ToString();
        await repository.Create(item);
        await cacheService.Invalidate($"ToDoItem:all");
    }

    public async Task UpdateAsync(ToDoItem item)
    {
        await repository.Update(item);
        await cacheService.Invalidate($"ToDoItem:{item.Id}");
        await cacheService.Invalidate($"ToDoItem:all");
    }

    public async Task DeleteAsync(string id)
    {
        await repository.Delete(id);
        await cacheService.Invalidate($"ToDoItem:{id}");
        await cacheService.Invalidate($"ToDoItem:all");
    }
}