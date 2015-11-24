using System;
using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    public class TestUserRepository : IUserAccountRepository<HierarchicalUserAccount>
    {
        private class TestUser : HierarchicalUserAccount
        {
            private static TestUser _testUser;
            private static TestUser _testAdmin;

            public static TestUser PreloadUser
            {
                get
                {
                    if (_testUser != null)
                    {
                        return _testUser;
                    }

                    _testUser = new TestUser
                    {
                        Created = new DateTime(2015, 11, 19, 16, 18, 47),
                        Email = "preload@fsw.com",
                        HashedPassword = "C350.AJ7NrlBZI/dudTuQHZn3nQOKp1OXO7tIfBFcm3kKiq1Kx/BcV0VNTm5I8prgb2d9mQ==",
                        ID = Guid.Parse("243dbc98-e273-43fc-a0d7-2976c939b1f0"),
                        IsAccountClosed = false,
                        IsAccountVerified = true,
                        IsLoginAllowed = true,
                        LastLogin = new DateTime(2015, 11, 19, 16, 20, 13),
                        LastUpdated = new DateTime(2015, 11, 19, 16, 20, 13),
                        PasswordChanged = new DateTime(2015, 11, 19, 16, 18, 47),
                        RequiresPasswordReset = false,
                        Tenant = "fsw",
                        Username = "Preload"
                    };

                    _testUser.AddClaim(new UserClaim("scope", "fsw_platform"));

                    return _testUser;
                }
            }

            public static TestUser TestAdmin
            {
                get
                {
                    if (_testAdmin != null)
                    {
                        return _testAdmin;
                    }

                    _testAdmin = new TestUser
                    {
                        Created = new DateTime(2015, 11, 19, 16, 18, 47),
                        Email = "admin@fsw.com",
                        HashedPassword = "C350.AJ7NrlBZI/dudTuQHZn3nQOKp1OXO7tIfBFcm3kKiq1Kx/BcV0VNTm5I8prgb2d9mQ==",
                        ID = new Guid(),
                        IsAccountClosed = false,
                        IsAccountVerified = true,
                        IsLoginAllowed = true,
                        LastLogin = new DateTime(2015, 11, 19, 16, 20, 13),
                        LastUpdated = new DateTime(2015, 11, 19, 16, 20, 13),
                        PasswordChanged = new DateTime(2015, 11, 19, 16, 18, 47),
                        RequiresPasswordReset = false,
                        Tenant = "fsw",
                        Username = "Admin"
                    };

                    _testAdmin.AddClaim(new UserClaim("scope", "fsw_platform"));
                    _testAdmin.AddClaim(new UserClaim("fsw:authcentral:admin", "true"));

                    return _testAdmin;
                }
            }
        }

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