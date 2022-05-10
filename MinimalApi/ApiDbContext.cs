using Microsoft.EntityFrameworkCore;

namespace MinimalApi
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Grocery> Groceries { get; set; }
    }
}
