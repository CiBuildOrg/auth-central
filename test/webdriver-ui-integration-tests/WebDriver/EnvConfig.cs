using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver
{
    public class EnvConfig : Dictionary<string,string>
    {
        private static class EnvVars
        {
            public static string RootUrl = "AUTHCENTRAL_ROOT_URL";
            public static string AdminUsername = "AUTHCENTRAL_ADMIN_USERNAME";
            public static string AdminPassword = "AUTHCENTRAL_ADMIN_PASSWORD";
            public static string NewUserUsername = "AUTHCENTRAL_NEWUSER_USERNAME";
            public static string NewUserPassword = "AUTHCENTRAL_NEWUSER_PASSWORD";
            public static string NewUserNewPassword = "AUTHCENTRAL_NEWUSER_NEWPASSWORD";
            public static string NewUserEmail = "AUTHCENTRAL_NEWUSER_EMAIL";
            public static string NewUserNewEmail = "AUTHCENTRAL_NEWUSER_NEWEMAIL";
            public static string NewUser_Outlook_Username = "AUTHCENTRAL_NEWUSER_OUTLOOKUSERNAME";
            public static string NewUser_Outlook_Password = "AUTHCENTRAL_NEWUSER_OUTLOOKPASSWORD";
        }

        private Dictionary<string, string> _config = new Dictionary<string, string>
        {
            { EnvVars.RootUrl, "https://secure.dev-fsw.com" },
            { EnvVars.AdminUsername, "AutomationUser" },
            { EnvVars.AdminPassword, "fs19!t?3h2@" },
            { EnvVars.NewUserUsername, "WebdriverNewUser" },
            { EnvVars.NewUserPassword, "J3huh@8h$$" },
            { EnvVars.NewUserNewPassword, "ih*GH3h*3" },
            { EnvVars.NewUserEmail, "automationsuser@fsw.com" },
            { EnvVars.NewUserNewEmail, "AUser2@fsw.com" },
            { EnvVars.NewUser_Outlook_Username, "automationsuser" },
            { EnvVars.NewUser_Outlook_Password, "##REPLACE_WITH_REAL_PASSWORD##" }
        };


        public string RootUrl { get { return _config[EnvVars.RootUrl]; } }
        public string AdminUser { get { return _config[EnvVars.AdminUsername]; } }
        public string AdminPassword { get { return _config[EnvVars.AdminPassword]; } }
        public string NewUserUsername { get { return _config[EnvVars.NewUserUsername]; } }
        public string NewUserPassword { get { return _config[EnvVars.NewUserPassword]; } }
        public string NewUserNewPassword { get { return _config[EnvVars.NewUserNewPassword]; } }
        public string NewUserEmail { get { return _config[EnvVars.NewUserEmail]; } }
        public string NewUserNewEmail { get { return _config[EnvVars.NewUserNewEmail]; } }
        public string NewUser_Outlook_Username { get { return _config[EnvVars.NewUser_Outlook_Username]; } }
        public string NewUser_Outlook_Password { get { return _config[EnvVars.NewUser_Outlook_Password]; } }

        public EnvConfig()
        {
            MergeEnvironmentVariables();
        }

        private Dictionary<string,string> MergeEnvironmentVariables()
        {
            IDictionary environment = Environment.GetEnvironmentVariables();
            foreach (string key in environment.Keys)
            {
                _config[key] = environment[key].ToString();
            }
            return _config;
        }
    }
}
