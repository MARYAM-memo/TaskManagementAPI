using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>(options)
{
      public DbSet<Models.Task> Tasks { get; set; }
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
            base.OnConfiguring(optionsBuilder);
      }
}
