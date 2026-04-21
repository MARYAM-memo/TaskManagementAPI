namespace TodoAPI.Interfaces;

public interface IUnitOfWork:IDisposable
{
      public IRepository<Models.Task> Tasks { get; }

      Task<int> SaveChangesAsync();
}
