using Microsoft.EntityFrameworkCore;
using TodoApi.Data.Enums;
using TodoApi.Data.Models;

namespace TodoApi.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();
                await SeedInitialData(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Произошла ошибка во время заполнения базы данных (сидинга).");
            }
        }

        private static async Task SeedInitialData(AppDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Email = "testuser1@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
                    },
                    new User
                    {
                        Email = "testuser2@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
                    }
                };
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            var user1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "testuser1@example.com");
            
            if (user1 == null) 
                return;

            if (!await context.Categories.AnyAsync(c => c.UserId == user1.Id))
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Работа", UserId = user1.Id },
                    new Category { Name = "Дом", UserId = user1.Id },
                    new Category { Name = "Хобби", UserId = user1.Id }
                };
                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                }

            var workCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Работа" && c.UserId == user1.Id);
            var homeCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Дом" && c.UserId == user1.Id);

            if (!await context.TodoItems.AnyAsync(t => t.UserId == user1.Id))
            {
                var todoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Title = "Подготовить отчет по проекту 'Альфа'",
                        Description = "Собрать данные, подготовить презентацию.",
                        Priority = PriorityLevel.High,
                        IsCompleted = false,
                        UserId = user1.Id,
                        CategoryId = workCategory?.Id 
                    },
                    new TodoItem
                    {
                        Title = "Купить молоко",
                        Priority = PriorityLevel.Medium,
                        IsCompleted = true,
                        UserId = user1.Id,
                        CategoryId = homeCategory?.Id
                    },
                    new TodoItem
                    {
                        Title = "Записаться к врачу",
                        Priority = PriorityLevel.High,
                        DueDate = DateTime.UtcNow.AddDays(3),
                        UserId = user1.Id,
                        CategoryId = homeCategory?.Id
                    },
                    new TodoItem
                    {
                        Title = "Прочитать главу книги по C#",
                        Description = "Глава про асинхронность.",
                        Priority = PriorityLevel.Low,
                        UserId = user1.Id
                    }
                };

                await context.TodoItems.AddRangeAsync(todoItems);
                await context.SaveChangesAsync();
            }
        }
    }
}