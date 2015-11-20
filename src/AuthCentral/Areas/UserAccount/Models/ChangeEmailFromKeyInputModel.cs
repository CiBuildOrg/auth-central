using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Models
{
    /// <summary>
    /// Model for the Confirm action of the <see cref="Areas.UserAccount.ChangeEmailController"/>
    /// </summary>
    public class ChangeEmailFromKeyInputModel
    {
        /// <summary>
        /// User's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Verification key set by the email verification request.
        /// </summary>
        [HiddenInput]
        public string Key { get; set; }
    }
}