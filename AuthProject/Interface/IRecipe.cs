using AuthProject.Dtos;
using AuthProject.Models;

namespace AuthProject.Interface
{
    public interface IRecipe
    {
        Task<Recipe> CreateRecipeAsync(string chefId, RecipeCreateDto dto);
        Task<List<object>> GetRecipesAsync(string? title);
    }
}
