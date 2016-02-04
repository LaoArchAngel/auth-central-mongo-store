using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Fsw.Enterprise.AuthCentral.MongoDb
{
    /// <summary>
    ///     Wrapper for the mongo database, supplying access to the Users and Groups collections.
    /// </summary>
    public class MongoDatabase
    {
        private readonly string _connectionString;

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

        /// <summary>
        ///     Initializes a new instance of the <see cref="MongoDatabase" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MongoDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        ///     Gets the 'groups' collection from Mongo and coverts them to <see cref="HierarchicalGroup" /> objects.
        /// </summary>
        /// <returns><see cref="MongoCollection{HierarchicalGroup}" /> of type <see cref="HierarchicalGroup" /></returns>
        public MongoCollection<HierarchicalGroup> Groups()
        {
            return GetCollection<HierarchicalGroup>("groups");
        }

        /// <summary>
        ///     Gets the 'users' collection from Mongo and converts them to <see cref="HierarchicalUserAccount" /> objects.
        /// </summary>
        /// <returns>
        ///     <see cref="MongoCollection{HierarchicalUserAccount}" /> of type <see cref="HierarchicalUserAccount" />
        /// </returns>
        public MongoCollection<HierarchicalUserAccount> Users()
        {
            return GetCollection<HierarchicalUserAccount>("users");
        }

        /// <summary>
        ///     Gets a collection with the given <paramref name="name" /> and converts them to objects of type
        ///     <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">Type to serialize the documents to from the mongo collection</typeparam>
        /// <param name="name">The name of the mongo collection.</param>
        /// <returns>A <see cref="MongoCollection{TDefaultDocument}" /> of type <typeparamref name="T" /></returns>
        public MongoCollection<T> GetCollection<T>(string name)
        {
            string databaseName = MongoUrl.Create(_connectionString).DatabaseName;
            var client = new MongoClient(_connectionString);

            // "GetServer(MongoClient) is obsolete: 'Use the new API instead.'"
            // The new API appears to be stricly asynchronous, 
            // which is not supported by upstream dependencies
#pragma warning disable 618
            MongoServer server = client.GetServer();
#pragma warning restore 618

            MongoDB.Driver.MongoDatabase database = server.GetDatabase(databaseName);
            return database.GetCollection<T>(name);
        }
    }
}