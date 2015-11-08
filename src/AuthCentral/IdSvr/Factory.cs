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

using System.Text.RegularExpressions;

using IdentityServer3.Core.Services;
using IdentityServer3.Core.Configuration;

// IdentityServer3 Store implementation
using AuthCentral.MongoStore;
using AuthCentral.MongoStore.Admin;

// IdentityServer3 User Service implementation
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
            var factory = new ServiceFactory(usrSrv, settings)
            {
                ViewService = new Registration<IViewService>(typeof (CustomViewService))
            };

            return factory;
        }

        public static IClientService GetClientService(string connectionString)
        {
            var settings = StoreSettings.DefaultSettings();
            settings.ConnectionString = connectionString;
            settings.Database = getDbNameFromMongoConnectionString(settings.ConnectionString);

            return (IClientService)AdminServiceFactory.CreateClientService(settings);
        }
        public static IScopeService GetScopeService(string connectionString)
        {
            var settings = StoreSettings.DefaultSettings();
            settings.ConnectionString = connectionString;
            settings.Database = getDbNameFromMongoConnectionString(settings.ConnectionString);

            return (IScopeService)AdminServiceFactory.CreateScopeService(settings);
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