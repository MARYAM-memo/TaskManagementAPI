using FluentValidation;
using TodoAPI.DTOs.Task;

namespace TodoAPI.Validators.Task;

public class TaskRequestValidator : AbstractValidator<TaskRequest>
{
      public TaskRequestValidator()
      {
            RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            
            .MaximumLength(80)
            .WithMessage("Title cannot exeed 80 character !");

            RuleFor(x => x.Discription)
            .MaximumLength(1200)
            .WithMessage("Description cannot exeed 1200 character !");

            RuleFor(x => x.DueDate)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Due date must be in the future");
      }
}
