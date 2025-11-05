using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs.Catigories;
using TodoApi.Services.Catigories;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCatigories()
        {
            var userId = GetUserId();
            var catigories = await _categoryService.GetAllAsync(userId);

            return Ok(catigories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var userId = GetUserId();
            var category = await _categoryService.GetByIdAsync(id, userId);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
        {
            try
            {
                var userId = GetUserId();
                var newCategory = await _categoryService.CreateAsync(dto, userId);

                return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            var userId = GetUserId();
            var (success, errorMessage) = await _categoryService.UpdateAsync(id, dto, userId);

            if(!success)
                return BadRequest(new {message = errorMessage});

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = GetUserId();
            var success = await _categoryService.DeleteAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}