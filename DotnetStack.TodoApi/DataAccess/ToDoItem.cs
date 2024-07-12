namespace DotnetStack.TodoApi.DataAccess;

public class ToDoItem
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
}