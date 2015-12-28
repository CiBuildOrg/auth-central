using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    public class UserAccountViewModel
    {
        public UserAccountViewModel() { }
        public UserAccountViewModel(HierarchicalUserAccount user)
        {
            // Required
            this.Email = user.Email;
            this.Username = user.Username;
            this.FirstName = user.GetClaimValue("given_name");
            this.LastName = user.GetClaimValue("family_name");
            this.LastUpdated = user.LastUpdated.ToUniversalTime();
            this.Created = user.Created.ToUniversalTime();

            // Optional
            this.PhoneNumber = user.GetClaimValue("phone_number");
            //this.ProfilePhotoUrl = new Uri(user.GetClaimValue("picture"));
            //this.Locale = System.Globalization.CultureInfo.GetCultureInfo(user.GetClaimValue("locale"));
            //this.Timezone = TimeZoneInfo.FindSystemTimeZoneById(user.GetClaimValue("zoneinfo"));
        }


        [ScaffoldColumn(false)]
        [Required]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set;  }

        public Uri ProfilePhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public System.TimeZoneInfo Timezone { get; set;  }
        public CultureInfo Locale { get; set; }
        [Required]
        public DateTimeOffset LastUpdated { get; set; }
        [Required]
        public DateTimeOffset Created { get; set; }
    }
}