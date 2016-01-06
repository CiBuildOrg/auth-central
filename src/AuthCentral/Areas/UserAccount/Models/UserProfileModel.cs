using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    public class UserProfileModel
    {
        public UserNameModel Name { get; set; }
        public ChangePasswordInputModel Password { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        public string Department { get; set; }
    }

    public class UserNameModel
    {
        public string UserId { get; set; }

        /// <summary>
        /// User's given (first) name.
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string GivenName { get; set; }

        /// <summary>
        /// User's family (last) name.
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string FamilyName { get; set; }

        public string Name { get { return GivenName + " " + FamilyName; } }
    }
}
