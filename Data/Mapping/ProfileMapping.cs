using AutoMapper;
using TodoAPI.DTOs.Task;

namespace TodoAPI.Data.Mapping;

public class ProfileMapping : Profile
{
      public ProfileMapping()
      {
            CreateMap<TaskRequest, Models.Task>();
            CreateMap<Models.Task, TaskResponse>();
      }
}
