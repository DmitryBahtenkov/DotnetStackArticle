using DotnetStack.UI.Models;

namespace DotnetStack.UI;

public class ToDoService(HttpClient httpClient)
{
    public async Task<List<ToDoItem>> GetToDoItemsAsync() 
        => await httpClient.GetFromJsonAsync<List<ToDoItem>>("todo");
    
    public async Task<ToDoItem> GetToDoItemByIdAsync(string id)
        => await httpClient.GetFromJsonAsync<ToDoItem>($"todo/{id}");

    public async Task CreateToDoItemAsync(ToDoItem item)
        => await httpClient.PostAsJsonAsync("todo", item);

    public async Task UpdateToDoItemAsync(ToDoItem item)
        => await httpClient.PutAsJsonAsync($"todo/{item.Id}", item);

    public async Task DeleteToDoItemAsync(string id)
        =>  await httpClient.DeleteAsync($"todo/{id}");
}