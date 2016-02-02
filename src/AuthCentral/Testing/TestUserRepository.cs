using System;
using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    internal class TestUserRepository : IUserAccountRepository<HierarchicalUserAccount>
    {
        private static Dictionary<Guid, HierarchicalUserAccount> _accounts;

        private Dictionary<Guid, HierarchicalUserAccount> Accounts
            => _accounts ?? (_accounts = new Dictionary<Guid, HierarchicalUserAccount>());

        public TestUserRepository()
        {
            if (!Accounts.ContainsKey(TestUser.PreloadUser.ID))
                Accounts.Add(TestUser.PreloadUser.ID, TestUser.PreloadUser);

            if(!Accounts.ContainsKey(TestUser.TestAdmin.ID))
                Accounts.Add(TestUser.TestAdmin.ID, TestUser.TestAdmin);
        }

        public HierarchicalUserAccount Create()
        {
            var account = new HierarchicalUserAccount();

            return account;
        }

        public void Add(HierarchicalUserAccount item)
        {
            if(Accounts.ContainsKey(item.ID))
                return;

            Accounts.Add(item.ID, item);
        }

        public void Remove(HierarchicalUserAccount item)
        {
            if (Accounts.ContainsKey(item.ID))
                Accounts.Remove(item.ID);
        }

        public void Update(HierarchicalUserAccount item)
        {
            if (Accounts.ContainsKey(item.ID))
                Accounts[item.ID] = item;
        }

        public HierarchicalUserAccount GetByUsername(string username)
        {
            return Accounts.Values.SingleOrDefault(a => a.Username == username);
        }

        public HierarchicalUserAccount GetByUsername(string tenant, string username)
        {
            return Accounts.Values.SingleOrDefault(a => a.Tenant == tenant && a.Username == username);
        }

        public HierarchicalUserAccount GetByEmail(string tenant, string email)
        {
            return Accounts.Values.SingleOrDefault(a => a.Email == email && a.Tenant == tenant);
        }

        public HierarchicalUserAccount GetByMobilePhone(string tenant, string phone)
        {
            return Accounts.Values.SingleOrDefault(a => a.Tenant == tenant && a.MobilePhoneNumber == phone);
        }

        public HierarchicalUserAccount GetByVerificationKey(string key)
        {
            return Accounts.Values.SingleOrDefault(a => a.VerificationKey == key);
        }

        public HierarchicalUserAccount GetByLinkedAccount(string tenant, string provider, string id)
        {
            return
                Accounts.Values.SingleOrDefault(
                    a =>
                        a.Tenant == tenant &&
                        a.LinkedAccounts.Any(
                            account => account.ProviderAccountID == id && account.ProviderName == provider));
        }

        public HierarchicalUserAccount GetByCertificate(string tenant, string thumbprint)
        {
            return
                Accounts.Values.SingleOrDefault(
                    account =>
                        account.Tenant == tenant &&
                        account.Certificates.Any(certificate => certificate.Thumbprint == thumbprint));
        }

        public HierarchicalUserAccount GetByID(Guid id)
        {
            return !Accounts.ContainsKey(id) ? null : Accounts[id];
        }
    }
}