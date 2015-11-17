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

using BrockAllen.MembershipReboot.Hierarchical;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.MembershipReboot;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    class Factory
    {
        public static IdentityServerServiceFactory Configure(Registration<IUserService, MembershipRebootUserService<HierarchicalUserAccount>> usrSrv)
        {
            var scopeStore = new InMemoryScopeStore(Scopes.Get());
            var clientStore = new InMemoryClientStore(Clients.Get());

            var factory = new IdentityServerServiceFactory
            {
                UserService = usrSrv,
                ScopeStore = new Registration<IScopeStore>(resolver => scopeStore),
                ClientStore = new Registration<IClientStore>(resolver => clientStore),
                CorsPolicyService = new Registration<ICorsPolicyService>(new DefaultCorsPolicyService {AllowAll = true})
            };

            return factory;
        }
    }
}