using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Models
{
    public class ChangeEmailRequestInputModel
    {
        //[Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}