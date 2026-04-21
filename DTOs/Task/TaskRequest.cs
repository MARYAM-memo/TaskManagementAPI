namespace TodoAPI.DTOs.Task;

public class TaskRequest
{
      public required string Title { get; set; }
      public string? Discription { get; set; }
      public DateTime DueDate { get; set; }
}
