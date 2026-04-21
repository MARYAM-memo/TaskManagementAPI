using System;
using TodoAPI.Data;
using TodoAPI.Interfaces;

namespace TodoAPI.DataAccess;

public class UnitOfWork : IUnitOfWork
{
      readonly DatabaseContext context;
      public UnitOfWork(DatabaseContext ctx)
      {
            context = ctx;
            Tasks = new Repository<Models.Task>(context);
      }
      public IRepository<Models.Task> Tasks { get; }

      public void Dispose()
      {
            context.Dispose();
            GC.SuppressFinalize(this);
      }

      public async Task<int> SaveChangesAsync()
      {
            return await context.SaveChangesAsync();
      }

}
