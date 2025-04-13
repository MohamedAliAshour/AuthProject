using System.ComponentModel.DataAnnotations;

namespace AuthProject.Dtos
{
    public class LoginDtos
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
