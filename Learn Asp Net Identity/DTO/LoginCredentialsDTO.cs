using System.ComponentModel.DataAnnotations;

namespace Learn_Asp_Net_Identity.DTO
{
    public class LoginCredentialsDTO
    {

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
