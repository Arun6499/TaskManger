using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TaskManger.Models;

namespace TaskManger.Data
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options) { }

        public virtual DbSet<Login> Logins { get; set; }  // Renamed to PascalCase
        public virtual DbSet<TaskModule> TaskModules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login"); 
                entity.HasKey(e => e.Id); 
            });
            modelBuilder.Entity<TaskModule>(entity =>
            {
                entity.ToTable("Task"); // Map TaskModule entity to Task table
                entity.HasKey(e => e.TaskId); // Define primary key
            });
        }    
    


    }
}
