using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.DTOs;
using TodoAPI.DTOs.Task;
using TodoAPI.Extensions;
using TodoAPI.Interfaces;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController(IUnitOfWork uow, IMapper iMapper) : ControllerBase
    {
        readonly IUnitOfWork unitOfWork = uow;
        readonly IMapper mapper = iMapper;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var query = unitOfWork.Tasks
              .GetQueryable()
              .Where(t => t.UserId == userId)
              .OrderByDescending(t => t.CreatedAt);

            var pagedTasks = await query
            .ProjectTo<TaskResponse>(mapper.ConfigurationProvider)
            .ToPagedResultAsync(pagination.PageNumber, pagination.PageSize);

            var dto = mapper.Map<List<TaskResponse>>(pagedTasks.Data);

            pagedTasks.Data = dto;
            return Ok(pagedTasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await unitOfWork.Tasks.FindAsync(id);
            if (task == null) return NotFound(new { message = "Task not found." });
            var dto = mapper.Map<TaskResponse>(task);
            return Ok(dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(TaskRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            //تحويل الريكويست للمودل 
            var task = mapper.Map<Models.Task>(request);

            task.CreatedAt = DateTime.UtcNow;
            task.UserId = userId;
            unitOfWork.Tasks.Add(task);
            await unitOfWork.SaveChangesAsync();

            var dto = mapper.Map<TaskResponse>(task);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("edit/{id:int}")]
        public async Task<IActionResult> EditTask(int id, TaskRequest request)
        {
            var task = await unitOfWork.Tasks.FindAsync(id);
            if (task == null) return NotFound(new { message = "Task not found." });
            mapper.Map(request, task);
            await unitOfWork.SaveChangesAsync();
            var dto = mapper.Map<TaskResponse>(task);
            return Ok(dto);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var task = await unitOfWork.Tasks.FindAsync(id);
            if (task == null) return NotFound(new { message = "Task not found." });
            if (task.UserId != userId)
                return Forbid();
            unitOfWork.Tasks.Remove(task);
            await unitOfWork.SaveChangesAsync();
            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
