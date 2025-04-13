using System.ComponentModel.DataAnnotations;

namespace AuthProject.Dtos
{
    public class RegisterDtos
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] roles { get; set; }
    }
}
