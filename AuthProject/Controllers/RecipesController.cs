using AuthProject.Data;
using AuthProject.Dtos;
using AuthProject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipesController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRecipe _recipeService;

        public RecipesController(
            AuthDbContext context,
            UserManager<IdentityUser> userManager,
            IRecipe recipeService)
        {
            _context = context;
            _userManager = userManager;
            _recipeService = recipeService;
        }

        // GET: api/recipes?title=optional
        [HttpGet]
        public async Task<IActionResult> GetRecipes([FromQuery] string? title)
        {
            var recipes = await _recipeService.GetRecipesAsync(title);
            return Ok(recipes);
        }

        // POST: api/recipes
        [HttpPost]
        [Authorize(Roles = "Chef")]
        public async Task<IActionResult> CreateRecipe([FromForm] RecipeCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var recipe = await _recipeService.CreateRecipeAsync(userId, dto);
                return Ok(new { message = "Recipe created"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create recipe", error = ex.Message });
            }
        }

        [HttpGet("images/{filename}")]
        public IActionResult GetImage(string filename)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Return 404 if the file doesn't exist
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var mimeType = "image/jpeg"; // You can detect mime type dynamically if needed

            return File(fileBytes, mimeType);
        }

    }
}
