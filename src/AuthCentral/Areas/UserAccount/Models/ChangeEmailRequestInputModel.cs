using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Models
{
    /// <summary>
    /// Input model for the index view of the <see cref="Areas.UserAccount.ChangeEmailController"/>
    /// </summary>
    public class ChangeEmailRequestInputModel
    {
        /// <summary>
        /// User's new email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}