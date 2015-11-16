using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models
{
    public class PasswordResetInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class PasswordResetWithSecretInputModel
    {
        private readonly IDataProtector _protector;

        public PasswordResetWithSecretInputModel (IDataProtector protector)
        {
            _protector = protector;
        }

        public PasswordResetWithSecretInputModel(IDataProtector protector, Guid accountID) : this(protector)
        {
            var bytes = Encoding.UTF8.GetBytes(accountID.ToString());
            bytes = _protector.Protect(bytes);
            ProtectedAccountID = Convert.ToBase64String(bytes);
        }

        public PasswordResetSecretViewModel[] Questions { get; set; }
        [Required]
        public string ProtectedAccountID { get; set; }

        public Guid? UnprotectedAccountID
        {
            get
            {
                try
                {
                    if (ProtectedAccountID != null)
                    {
                        var bytes = Convert.FromBase64String(ProtectedAccountID);
                        bytes = _protector.Unprotect(bytes);
                        var val = Encoding.UTF8.GetString(bytes);
                        return Guid.Parse(val);
                    }
                }
                catch { }
                return null;
            }
        }
    }

    public class PasswordResetSecretViewModel : PasswordResetSecretInputModel
    {
        public string Question { get; set; }
    }

    public class PasswordResetSecretInputModel
    {
        [HiddenInput]
        public Guid QuestionID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Answer { get; set; }
    }
}