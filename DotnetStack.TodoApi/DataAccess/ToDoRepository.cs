using Raven.Client.Documents;

namespace DotnetStack.TodoApi.DataAccess;

public class ToDoRepository(IDocumentStore store)
{
    public async Task<IEnumerable<ToDoItem>> GetAll()
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<ToDoItem>().ToListAsync();
    }

    public async Task<ToDoItem> GetById(string id)
    {
        using var session = store.OpenAsyncSession();
        return await session.LoadAsync<ToDoItem>(id);
    }

    public async Task Create(ToDoItem item)
    {
        using var session = store.OpenAsyncSession();
        await session.StoreAsync(item);
        await session.SaveChangesAsync();
    }

    public async Task Update(ToDoItem item)
    {
        using var session = store.OpenAsyncSession();
        session.Advanced.Patch(item, x => x.Deadline, item.Deadline);
        session.Advanced.Patch(item, x => x.Title, item.Title);
        session.Advanced.Patch(item, x => x.Description, item.Description);
        await session.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        using var session = store.OpenAsyncSession();
        session.Delete(id);
        await session.SaveChangesAsync();
    }
}