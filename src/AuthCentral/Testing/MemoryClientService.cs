using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    internal class MemoryClientService : IClientService
    {
        private static readonly Dictionary<string, Client> Clients = Testing.Clients.Get().ToDictionary(client => client.ClientId);

        /// <summary>
        ///     Saves either a new or existing <see cref="Client" /> to the store.
        /// </summary>
        /// <param name="client"></param>
        public async Task Save(Client client)
        {
            if (Clients.ContainsKey(client.ClientId))
            {
                Clients[client.ClientId] = client;
            }
            else
            {
                Clients.Add(client.ClientId, client);
            }
        }

        /// <summary>
        ///     Finds the client with the given id.
        /// </summary>
        /// <param name="clientId">The unique id of the client. This is the value passed in by the client when authenticating.</param>
        /// <returns>A <see cref="Client" /> with the given <paramref name="clientId" /></returns>
        public async Task<Client> Find(string clientId)
        {
            return Clients.ContainsKey(clientId) ? Clients[clientId] : null;
        }

        /// <summary>
        ///     Removes the <see cref="Client" /> from the store with the given <paramref name="clientId" />
        /// </summary>
        /// <param name="clientId">The unique id of the client. This is the value passed in by the client when authenticating.</param>
        public async Task Delete(string clientId)
        {
            Clients.Remove(clientId);
        }

        public async Task<ClientPagingResult> GetPageAsync(int pageNumber, int rowsPerPage)
        {
            var result = new ClientPagingResult
            {
                Collection = Clients.Values,
                HasMore = false
            };

            return result;
        }

        public async Task<ClientPagingResult> GetRangeAsync(int offset, int limit)
        {
            var result = new ClientPagingResult
            {
                Collection = Clients.Values.Skip(offset).Take(limit),
                HasMore = (Clients.Count - offset - limit) > 0
            };

            return result;
        }
    }
}