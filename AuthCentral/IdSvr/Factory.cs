/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Text.RegularExpressions;

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.MongoDb;
using IdentityServer3.MembershipReboot;

using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdSvr
{
    static class Factory
    {
        private const string pattern = @"^mongodb://.+?/(.+?)(?:\?(.+=.+)+$|$)";
        private static readonly Regex r = new Regex(pattern);

        public static IdentityServerServiceFactory Configure(string connectionString)
        {
            var settings = StoreSettings.DefaultSettings();

            settings.ConnectionString = connectionString;
            settings.Database = getDbNameFromMongoConnectionString(connectionString);

            var usrSrv = new Registration<IUserService, MembershipRebootUserService<HierarchicalUserAccount>>();
            var factory = new ServiceFactory(usrSrv, settings);
            // factory.ViewService = new Registration<IViewService>(typeof(CustomViewService));

            return factory;
        }

        private static string getDbNameFromMongoConnectionString(string connectionString)
        {
            string result = "identityserver";

            Match match = r.Match(connectionString);

            if(match.Success)
            {
                result = match.Groups[1].Value;
            }

            return result;
        }
    }
}