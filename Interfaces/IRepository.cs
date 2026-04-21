using System;

namespace TodoAPI.Interfaces;

public interface IRepository<T> where T : class
{
      Task<IEnumerable<T>> GetAllAsync(bool withNoTracking = false);
      Task<T?> FindAsync(int id);
      void Add(T entity);
      void Remove(T entry);
      IQueryable<T> GetQueryable();
      int SaveChanges();
}
