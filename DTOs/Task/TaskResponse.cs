using System;

namespace TodoAPI.DTOs.Task;

public class TaskResponse
{
      public int Id { get; set; }
      public int UserId { get; set; }
      public required string Title { get; set; }
      public string? Discription { get; set; }
      public DateTime DueDate { get; set; }
      public bool IsCompleted { get; set; }
      public DateTime CreatedAt { get; set; }
}
