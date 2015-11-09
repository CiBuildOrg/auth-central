using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Models
{
    public class ChangeEmailFromKeyInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput]
        public string Key { get; set; }
    }
}