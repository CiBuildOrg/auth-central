using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    /// <summary>
    /// Model for the confirm action of the PasswordReset controller.
    /// </summary>
    public class ChangePasswordFromResetKeyInputModel
    {
        /// <summary>
        /// User's new password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        /// <summary>
        /// Confirm password.  Must match <see cref="Password"/> or it will show an error.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Password confirmation must match password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        
        /// <summary>
        /// Verification key passed in as a GET parameter
        /// </summary>
        [HiddenInput]
        public string Key { get; set; }
    }
}