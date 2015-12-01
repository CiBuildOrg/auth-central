using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class CreateAccountInputModel
    {
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
    }
}