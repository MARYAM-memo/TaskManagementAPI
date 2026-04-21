using System;
using Microsoft.EntityFrameworkCore;
using TodoAPI.DTOs;

namespace TodoAPI.Extensions;

public static class PaginationExtensions
{
      public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
              this IQueryable<T> query,
              int pageNumber,
              int pageSize)
      {
            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                  PageNumber = pageNumber,
                  PageSize = pageSize,
                  TotalCount = totalCount,
                  TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                  Data = data
            };
      }
}
