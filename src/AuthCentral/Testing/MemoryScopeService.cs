using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    public class MemoryScopeService : IScopeService
    {
        private static readonly Dictionary<string, Scope> Scopes = Testing.Scopes.Get()
            .ToDictionary(scope => scope.Name);

        public Task Save(Scope scope)
        {
            if (Scopes.ContainsKey(scope.Name))
                Scopes[scope.Name] = scope;
            else
            {
                Scopes.Add(scope.Name, scope);
            }

            return Task.FromResult(0);
        }

        public Task Delete(string scopeName)
        {
            Scopes.Remove(scopeName);

            return Task.FromResult(0);
        }

        public Task<Scope> Find(string scopeName)
        {
            return Scopes.ContainsKey(scopeName) ? Task.FromResult(Scopes[scopeName]) : null;
        }

        public Task<IEnumerable<Scope>> Find(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(Scopes.Where(pair => scopeNames.Contains(pair.Key)).Select(pair => pair.Value));
        }
    }
}