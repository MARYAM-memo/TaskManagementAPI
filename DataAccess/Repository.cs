using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Interfaces;
namespace TodoAPI.DataAccess;

public class Repository<T>(DatabaseContext context) : IRepository<T> where T : class
{
      readonly DatabaseContext ctx = context;

      public void Add(T entity)
      {
            ctx.Set<T>().Add(entity);
      }

      public async Task<T?> FindAsync(int id)
      {
            return await ctx.Set<T>().FindAsync(id);
      }

      public async Task<IEnumerable<T>> GetAllAsync(bool withNoTracking = false)
      {
            if (withNoTracking)
                  return await ctx.Set<T>().AsNoTracking().ToListAsync();
            else
                  return await ctx.Set<T>().ToListAsync();
      }

      public IQueryable<T> GetQueryable()
      {
            return ctx.Set<T>().AsQueryable();
      }

      public void Remove(T entry)
      {
            ctx.Set<T>().Remove(entry);
      }

      public int SaveChanges()
      {
            return ctx.SaveChanges();
      }
}
