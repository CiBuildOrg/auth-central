using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    /// <summary>
    /// Input model for the Password Reset flow.
    /// </summary>
    public class PasswordResetInputModel
    {
        /// <summary>
        /// User's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}