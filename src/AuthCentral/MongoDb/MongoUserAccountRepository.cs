using System;
using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using Fsw.Enterprise.AuthCentral.IdMgr;

namespace Fsw.Enterprise.AuthCentral.MongoDb
{
    public class MongoUserAccountRepository<T> : QueryableUserAccountRepository<T>, IBulkUserRepository<T>
        where T : HierarchicalUserAccount, new()
    {
        private readonly MongoDatabase _db;

        public MongoUserAccountRepository(MongoDatabase db)
        {
            UseEqualsOrdinalIgnoreCaseForQueries = true;
            _db = db;
        }

        //protected override IQueryable<T> Queryable => (IQueryable<T>) _db.Users().FindAll().AsQueryable();
        protected override IQueryable<T> Queryable
        {
            get { 
                return (IQueryable<T>) _db.Users().FindAll().AsQueryable();
            }
        }

        public IEnumerable<T> GetPagedUsers(int page, int pageSize, out long count)
        {
            // Optional parameters must appear after all other parameters
            if (page < 1) { page = 1; }
            if (pageSize < 1) { pageSize = 25; }

            var accountsQuery = _db.Users().FindAllAs<T>();
            count = accountsQuery.Count();

            // MongoCursor.Limit(x) restricts how many records can be retrieved
            // But MongoCursor.Skip enumerates the list, removes the first N and returns the rest as an IEnumerable<T>
            // This enumeration counts towards Limit. Once Skip is called, Limit is no longer useful.
            // Therefore to our Limit we have to add the number of records that will be skipped.
            accountsQuery.Limit = page * pageSize;
            IEnumerable<T> accounts = accountsQuery.Skip((page - 1) * pageSize);

            return accounts;
        }

        public override T Create()
        {
            return new T();
        }

        public override void Add(T item)
        {
            _db.Users().Insert(item);
        }

        public override void Update(T item)
        {
            _db.Users().Save(item);
        }

        public override void Remove(T item)
        {
            _db.Users().Remove(Query<T>.EQ(e => e.ID, item.ID));
        }

        public override T GetByLinkedAccount(string tenant, string provider, string id)
        {
            var query =
                from a in Queryable
                where a.Tenant == tenant
                from la in a.LinkedAccountCollection
                where la.ProviderName == provider && la.ProviderAccountID == id
                select a;
            return query.SingleOrDefault();
        }

        public override T GetByCertificate(string tenant, string thumbprint)
        {
            var query =
                from a in Queryable
                where a.Tenant == tenant
                from c in a.UserCertificateCollection
                where c.Thumbprint == thumbprint
                select a;
            return query.SingleOrDefault();
        }

        //IUserAccountRepository<T> This { get { return (IUserAccountRepository<T>)this; } }


        //UserAccount IUserAccountRepository<UserAccount>.Create()
        //{
        //    return This.Create();
        //}

        //void IUserAccountRepository<UserAccount>.Add(UserAccount item)
        //{
        //    This.Add((T)item);
        //}

        //void IUserAccountRepository<UserAccount>.Remove(UserAccount item)
        //{
        //    This.Remove((T)item);
        //}

        //void IUserAccountRepository<UserAccount>.Update(UserAccount item)
        //{
        //    This.Update((T)item);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByID(Guid id)
        //{
        //    return This.GetByID(id);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByUsername(string username)
        //{
        //    return This.GetByUsername(username);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByUsername(string tenant, string username)
        //{
        //    return This.GetByUsername(tenant, username);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByEmail(string tenant, string email)
        //{
        //    return This.GetByEmail(tenant, email);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByMobilePhone(string tenant, string phone)
        //{
        //    return This.GetByMobilePhone(tenant, phone);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByVerificationKey(string key)
        //{
        //    return This.GetByVerificationKey(key);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByLinkedAccount(string tenant, string provider, string id)
        //{
        //    return This.GetByLinkedAccount(tenant, provider, id);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByCertificate(string tenant, string thumbprint)
        //{
        //    return This.GetByCertificate(tenant, thumbprint);
        //}
    }
}
