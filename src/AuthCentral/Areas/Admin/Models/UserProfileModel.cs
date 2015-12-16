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

    }
}
