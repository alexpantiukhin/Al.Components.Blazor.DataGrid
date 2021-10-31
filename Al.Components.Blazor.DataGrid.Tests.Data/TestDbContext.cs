using Microsoft.EntityFrameworkCore;

namespace Al.Components.Blazor.DataGrid.Tests.Data
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        public TestDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("testdb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasData(
                    new User { Id = 1, FirstName = "Вася" },
                    new User { Id = 2, FirstName = "Петя" },
                    new User { Id = 3, FirstName = "Иван" },
                    new User { Id = 4, FirstName = "Вася" },
                    new User { Id = 5, FirstName = "Петя" },
                    new User { Id = 6, FirstName = "Вася" },
                    new User { Id = 7, FirstName = "Сергей" },
                    new User { Id = 8, FirstName = "Иван" },
                    new User { Id = 9, FirstName = "Вася" },
                    new User { Id = 10, FirstName = "Вася" },
                    new User { Id = 11, FirstName = "Вася" }
                );
        }
    }
}