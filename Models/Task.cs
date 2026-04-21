using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TodoAPI.Models;

public class Task
{
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public required string Title { get; set; }
      public string? Discription { get; set; }
      public DateTime DueDate { get; set; }
      public bool IsCompleted { get; set; }
      public DateTime CreatedAt { get; set; }

      //foreign key
      public int UserId { get; set; }
      public IdentityUser<int>? User { get; set; }
}
