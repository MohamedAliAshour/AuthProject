using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthProject.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string? ImagePath { get; set; } // Store image file path instead of image data

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ChefId { get; set; }
        public IdentityUser Chef { get; set; }
    }

}
