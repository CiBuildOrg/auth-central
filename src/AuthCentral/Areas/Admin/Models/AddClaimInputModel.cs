using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Models
{
    public class AddClaimInputModel
    {
        [Required]
        public string ClaimType { get; set; }
        
        [Required]
        public string ClaimValue { get; set; }
        
    }
}