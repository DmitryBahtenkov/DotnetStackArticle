using System.ComponentModel.DataAnnotations;

namespace DotnetStack.UI.Models;

public class ToDoItem
{
    public string Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
}