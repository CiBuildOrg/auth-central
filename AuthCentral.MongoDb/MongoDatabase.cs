using System.Configuration;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Fsw.Enterprise.AuthCentral.MongoDb
{
    public class MongoDatabase
    {
        static MongoDatabase()
        {
            BsonClassMap.RegisterClassMap<Group>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.ID);
            });

            BsonClassMap.RegisterClassMap<UserAccount>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.ID);
            });

            BsonClassMap.RegisterClassMap<HierarchicalUserAccount>(cm => cm.AutoMap());
            BsonClassMap.RegisterClassMap<HierarchicalGroup>(cm => cm.AutoMap());
        }

        private readonly string _connectionString;

        public MongoDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MongoCollection<HierarchicalGroup> Groups()
        {
            return GetCollection<HierarchicalGroup>("groups");
        }

        public MongoCollection<HierarchicalUserAccount> Users()
        {
            return GetCollection<HierarchicalUserAccount>("users");
        }

        public MongoCollection<T> GetCollection<T>(string name)
        {
            var databaseName = MongoUrl.Create(_connectionString).DatabaseName;
            var client = new MongoClient(_connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(databaseName);
            return database.GetCollection<T>(name);
        }
    }
}
