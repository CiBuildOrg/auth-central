﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver
{
    class EnvConfig : Dictionary<string,string>
    {
        private static class EnvVars
        {
            public static string RootUrl = "AUTHCENTRAL_ROOT_URL";
            public static string AdminUsername = "AUTHCENTRAL_ADMIN_USERNAME";
            public static string AdminPassword = "AUTHCENTRAL_ADMIN_PASSWORD";
        }

        private Dictionary<string, string> _config = new Dictionary<string, string>
        {
            { EnvVars.RootUrl, "https://secure.dev-fsw.com/" },
            { EnvVars.AdminUsername, "jburbage" },
            { EnvVars.AdminPassword, "n1c37rY" }
        };


        public string RootUrl { get { return _config[EnvVars.RootUrl]; } }
        public string AdminUser { get { return _config[EnvVars.AdminUsername]; } }
        public string AdminPassword { get { return _config[EnvVars.AdminPassword]; } }
        
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
