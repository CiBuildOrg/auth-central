using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    /// <summary>
    /// Input model for the Admin - Create Account view form.
    /// </summary>
    public class CreateAccountInputModel
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
        /// Whether this user is an AuthCentral administrator.
        /// </summary>
        public bool IsAuthCentralAdmin { get; set; }

        /// <summary>
        /// Organization that this user belongs to. FSW employees should be 'FSW'.
        /// </summary>
        public string Organization { get; set; }
        
        /// <summary>
        /// Department within the organization.
        /// </summary>
        public string Department { get; set; }
        
    }
}