using MongoDB.Driver;

namespace AuthCentral.MongoStore.Admin
{
    public static class AdminServiceFactory
    {
        public static IDatabaseService CreateClientService(StoreSettings settings)
        {
            return Create(settings) as IDatabaseService;
        }

        public static IDatabaseService CreateDatabaseService(StoreSettings settings)
        {
            return Create(settings) as IDatabaseService;
        }

        public static IDatabaseService CreateScopeService(StoreSettings settings)
        {
            return Create(settings) as IDatabaseService;
        }
   
        private static AdminService Create(StoreSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var db = mongoClient.GetDatabase(settings.Database);
            return new AdminService(mongoClient, db, settings);
        }
    }
}
