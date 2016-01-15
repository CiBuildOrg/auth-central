using System;
using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot;
using MongoDB.Driver.Builders;

namespace Fsw.Enterprise.AuthCentral.MongoDb
{
    public class MongoGroupRepository<T> :
        QueryableGroupRepository<T>
        where T : HierarchicalGroup, new()
    {
        private readonly MongoDatabase _db;

        public MongoGroupRepository(MongoDatabase db)
        {
            _db = db;
        }

        protected override IQueryable<T> Queryable
        {
            get { 
                return (IQueryable<T>) _db.Groups().FindAll().AsQueryable();
            }
        }

        public override T Create()
        {
            return new T();
        }

        public override void Add(T item)
        {
            _db.Groups().Insert(item);
        }

        public override void Remove(T item)
        {
            _db.Groups().Remove(Query<T>.EQ(e => e.ID, item.ID));
        }

        public override void Update(T item)
        {
            _db.Groups().Save(item);
        }

        public override IEnumerable<T> GetByChildID(Guid childGroupId)
        {
            var q =
                from g in Queryable
                from c in g.Children
                where c.ChildGroupID == childGroupId
                select g;
            return q;
        }
    }
}
