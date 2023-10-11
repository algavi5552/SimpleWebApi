namespace Simple_REST_API_App
{
    using Microsoft.EntityFrameworkCore;
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Name = "Pavel", Age = 34 },
                    new User { Id = 2, Name = "July", Age = 29 },
                    new User { Id = 3, Name = "Alex", Age = 22 }
            );
        }
    }
}
