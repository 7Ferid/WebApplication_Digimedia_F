using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApplication_Digimedia_F.Models;

namespace WebApplication_Digimedia_F.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
