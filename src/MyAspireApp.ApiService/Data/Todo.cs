namespace MyAspireApp.ApiService.Data;

public class Todo
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required bool IsDone { get; set; }
}

public record CreateTodo(string Title);