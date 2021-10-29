using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    public class TestDbContext : DbContext
    {
        public DbSet<A> As { get; set; }
        public DbSet<B> Bs { get; set; }

        public TestDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=test.db");
        }
    }
}
