using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Data.Models;
using TodoApi.DTOs.Catigories;

namespace TodoApi.Services.Catigories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, int userId)
        {
            var existingCategory = await _context.Categories
                .AnyAsync(c => c.UserId == userId && c.Name.ToLower() == dto.Name.ToLower());

            if (existingCategory)
                throw new InvalidOperationException("Категория с таким названием уже существует.");

            var category = new Category
            {
                Name = dto.Name,
                UserId = userId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(int userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id, int userId)
        {
            return await _context.Categories
                .Where(c => c.Id == id && c.UserId == userId)
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                .FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, UpdateCategoryDto dto, int userId)
        {
            var categoryToUpdate = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (categoryToUpdate == null)
                return (false, "Категория не найдена или не принадлежит вам.");

            var nameExists = await _context.Categories
                .AnyAsync(c => c.UserId == userId && c.Id != id && c.Name.ToLower() == dto.Name.ToLower());

            if (nameExists)
                return (false, "Категория с таким названием уже существует.");

            categoryToUpdate.Name = dto.Name;
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}