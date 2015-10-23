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

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.MongoDb;
using IdentityManager;
using IdentityManager.Configuration;
using IdentityManager.MembershipReboot;
using Microsoft.AspNet.Builder;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public static class MembershipRebootIdentityManagerServiceExtensions
    {
        public static void Configure(this IdentityManagerServiceFactory factory, IApplicationBuilder app, string connectionString)
        {
            factory.IdentityManagerService = new Registration<IIdentityManagerService, MembershipRebootIdentityManagerService<HierarchicalUserAccount, HierarchicalGroup>>();
            factory.Register(new Registration<UserAccountService<HierarchicalUserAccount>>());
            factory.Register(new Registration<GroupService<HierarchicalGroup>>());
            factory.Register(new Registration<IUserAccountRepository<HierarchicalUserAccount>,  MongoUserAccountRepository<HierarchicalUserAccount>>());
            factory.Register(new Registration<IGroupRepository<HierarchicalGroup>, MongoGroupRepository<HierarchicalGroup>>());
            factory.Register(new Registration<MongoDatabase>(resolver => new MongoDatabase(connectionString)));
            factory.Register(new Registration<MembershipRebootConfiguration<HierarchicalUserAccount>>(MembershipRebootSetup.GetConfig(app)));
        }
    }
}