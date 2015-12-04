using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    /// <summary>
    /// Input model for the Register Index view form.
    /// </summary>
    public class RegisterInputModel
    {
        /// <summary>
        /// New username.
        /// </summary>
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        
        /// <summary>
        /// User's email address.  Must be unique for the Tenant.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User's given (first) name.
        /// </summary>
        [Required]
        public string GivenName { get; set; }

        /// <summary>
        /// User's middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// User's family (last) name.
        /// </summary>
        [Required]
        public string FamilyName { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        /// <summary>
        /// Password confirmation.  Must match <see cref="Password"/> or it will return an error.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage="Password confirmation must match password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}