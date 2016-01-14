using System.ComponentModel.DataAnnotations;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    /// <summary>
    /// Input model for the index view of the <see cref="ChangeEmailController"/>
    /// </summary>
    public class ChangeEmailRequestInputModel
    {
        /// <summary>
        /// User's new email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "New E-mail")]
        public string NewEmail { get; set; }
    }
}