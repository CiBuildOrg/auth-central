using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class UserProfileModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// User's given (first) name.
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string GivenName { get; set; }

        /// <summary>
        /// User's middle name.
        /// </summary>
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// User's family (last) name.
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// User's Organization (e.g. FSW)
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// User's department (e.g. Technology)
        /// </summary>
        public string Department { get; set; }

        public bool IsLoginAllowed { get; set; }

    }
}
