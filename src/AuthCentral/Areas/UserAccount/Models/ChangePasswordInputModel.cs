using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    public class ChangePasswordInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Confirm New Password")]
        public string NewPasswordConfirm { get; set; }
    }
}