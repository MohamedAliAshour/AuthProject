using AuthProject.Data;
using AuthProject.Dtos;
using AuthProject.Interface;
using AuthProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthProject.Services
{
    public class RecipeService : IRecipe
    {
        private readonly IWebHostEnvironment _env;
        private readonly AuthDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipeService(IWebHostEnvironment env, AuthDbContext context, IHttpContextAccessor accessor)
        {
            _env = env;
            _context = context;
            _httpContextAccessor = accessor;
        }

        public async Task<Recipe> CreateRecipeAsync(string chefId, RecipeCreateDto dto)
        {
            string? imagePath = null;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                const int maxFileSize = 5 * 1024 * 1024;
                if (dto.ImageFile.Length > maxFileSize)
                    throw new Exception("File too large. Max 5MB.");

                var fileExt = Path.GetExtension(dto.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExt}";
                var folderPath = Path.Combine(_env.WebRootPath, "Images");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                var request = _httpContextAccessor.HttpContext.Request;
                imagePath = $"{request.Scheme}://{request.Host}/Images/{fileName}";
            }

            var recipe = new Recipe
            {
                Title = dto.Title,
                Description = dto.Description,
                ImagePath = imagePath,
                ChefId = chefId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }

        public async Task<List<object>> GetRecipesAsync(string? title)
        {
            var query = _context.Recipes.Include(r => r.Chef).AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(r => r.Title.Contains(title));
            }

            var recipes = await query.Select(r => new
            {
                r.Id,
                r.Title,
                r.Description,
                Chef = r.Chef.UserName,
                r.CreatedAt,
                ImageUrl = r.ImagePath
            }).ToListAsync();

            return recipes.Cast<object>().ToList();
        }
    }
}
