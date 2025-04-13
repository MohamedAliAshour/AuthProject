using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthProject.Dtos
{
    public class RecipeCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? FileName { get; set; }

        public string? FileDescription { get; set; }

    }
}
