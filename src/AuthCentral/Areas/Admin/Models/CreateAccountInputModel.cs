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
        
    }
}