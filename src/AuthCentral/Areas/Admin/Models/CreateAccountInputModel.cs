using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Models
{
    public class CreateAccountInputModel
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage="Password confirmation must match password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
    }
}