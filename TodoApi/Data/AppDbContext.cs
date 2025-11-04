using Microsoft.EntityFrameworkCore;
using TodoApi.Data.Enums;
using TodoApi.Data.Models;

namespace TodoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        DbSet<User> Users { get; set; }
        DbSet<TodoItem> TodoItems { get; set; }
        DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);

                user.Property(u => u.Username).IsRequired().HasMaxLength(50);
                user.HasIndex(u => u.Username).IsUnique(); 
                user.Property(u => u.Email).IsRequired().HasMaxLength(100);
                user.HasIndex(u => u.Email).IsUnique(); 
                user.Property(u => u.PasswordHash).IsRequired();
                
                user.HasMany(u => u.TodoItems)      
                    .WithOne(t => t.User)           
                    .HasForeignKey(t => t.UserId)   
                    .OnDelete(DeleteBehavior.NoAction);

                user.HasMany(u => u.Categories)     
                    .WithOne(c => c.User)           
                    .HasForeignKey(c => c.UserId)   
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(category =>
            {
                category.HasKey(c => c.Id);
                category.Property(c => c.Name).IsRequired().HasMaxLength(100);

                category.HasMany(c => c.TodoItems)  
                        .WithOne(t => t.Category)       
                        .HasForeignKey(t => t.CategoryId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TodoItem>(item =>
            {
                item.HasKey(t => t.Id);
                item.Property(t => t.Title).IsRequired().HasMaxLength(200);
                item.Property(t => t.Description).IsRequired(false);
                item.Property(t => t.IsCompleted).HasDefaultValue(false);
                item.Property(t => t.Priority).HasDefaultValue(PriorityLevel.Medium);
                item.Property(t => t.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}